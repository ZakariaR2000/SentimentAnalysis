//using SentimentAnalysis2;
//using System;

//namespace SentimentAnalysis
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var data = new[]
//            {
//                new clsSentimentData { Text = "This product is amazing!", Sentiment = 1 },
//                new clsSentimentData { Text = "I hate this!", Sentiment = 0 },
//                new clsSentimentData { Text = "Worst experience ever!", Sentiment = 0 },
//                new clsSentimentData { Text = "Absolutely fantastic!", Sentiment = 1 },
//                new clsSentimentData { Text = "Not bad, but could be better.", Sentiment = 1 }
//            };

//            var model = new clsLogisticRegressionModel();
//            model.Train(data, learningRate: 0.01, epochs: 1000);

//            Console.WriteLine("\n🔹 Model Evaluation 🔹");
//            model.Evaluate(data);

//            Console.WriteLine("\n🔹 Predictions 🔹");
//            foreach (var item in data)
//            {
//                var prediction = model.Predict(item.Text) ? "Positive 😊" : "Negative 😡";
//                Console.WriteLine($"Comment: {item.Text}\nPrediction: {prediction}\n");
//            }
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using SentimentAnalysis2;

namespace SentimentAnalysis2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                // ✅ 1. الاتصال بواجهة YouTube API
                UserCredential credential;
                using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
                {
                    credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                            GoogleClientSecrets.FromStream(stream).Secrets,
                            new[] {
                                "https://www.googleapis.com/auth/youtube.readonly",
                                "https://www.googleapis.com/auth/youtube.force-ssl"
                            },
                            "user",
                            System.Threading.CancellationToken.None,
                            new FileDataStore("YouTubeSentimentAuthStore", true)
                        );
                                                
                }
                ;

                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "SentimentAnalysisApp"
                });

                Console.WriteLine("✅ Connected to YouTube API!");

                // ✅ 2. إدخال معرف الفيديو
                Console.Write("🎥 Enter YouTube video ID: ");
                string videoId = Console.ReadLine();

                // ✅ 3. جلب التعليقات من YouTube
                var commentRequest = youtubeService.CommentThreads.List("snippet");
                commentRequest.VideoId = videoId;
                commentRequest.TextFormat = CommentThreadsResource.ListRequest.TextFormatEnum.PlainText;
                commentRequest.MaxResults = 10;

                var commentResponse = await commentRequest.ExecuteAsync();
                var comments = commentResponse.Items.Select(c => c.Snippet.TopLevelComment.Snippet.TextDisplay).ToList();

                Console.WriteLine("\n🗨️ Comments Fetched from YouTube:");
                foreach (var comment in comments)
                    Console.WriteLine("- " + comment);

                // ✅ 4. تعريف بيانات التدريب
                var trainingData = new[]
                {
                    new clsSentimentData { Text = "This product is amazing!", Sentiment = 1 },
                    new clsSentimentData { Text = "I love this!", Sentiment = 1 },
                    new clsSentimentData { Text = "I hate this!", Sentiment = 0 },
                    new clsSentimentData { Text = "Worst experience ever!", Sentiment = 0 },
                    new clsSentimentData { Text = "Absolutely fantastic!", Sentiment = 1 },
                    new clsSentimentData { Text = "Not bad, but could be better.", Sentiment = 1 },
                    new clsSentimentData { Text = "I am very disappointed.", Sentiment = 0 },
                    new clsSentimentData { Text = "Horrible, I regret buying this!", Sentiment = 0 },
                    new clsSentimentData { Text = "Fantastic work, I'm very impressed!", Sentiment = 1 },
                    new clsSentimentData { Text = "Awful! Complete waste of money!", Sentiment = 0 }
                };

                // ✅ 5. تدريب النموذج
                var model = new clsLogisticRegressionModel();
                model.Train(trainingData, learningRate: 0.01, epochs: 1000);

                Console.WriteLine("\n📊 Model Evaluation:");
                model.Evaluate(trainingData);

                // ✅ 6. توقع مشاعر التعليقات المجلوبة من YouTube
                Console.WriteLine("\n💬 Sentiment Analysis Results:");
                foreach (var comment in comments)
                {
                    bool prediction = model.Predict(comment);
                    string sentiment = prediction ? "Positive 😊" : "Negative 😡";
                    Console.WriteLine($"Comment: {comment}\nPrediction: {sentiment}\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Error: " + ex.Message);
            }
        }
    }
}
