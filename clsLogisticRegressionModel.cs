//using SentimentAnalysis2;
//using System;
//using System.Collections.Generic;
//using System.Linq;

///*
//تدريب النموذج باستخدام Gradient Descent.
//استخدام دالة Sigmoid لتحويل القيم إلى احتمالات.
//التنبؤ (Prediction) بالنتائج (Positive/Negative).
//تحديث المعاملات (Weights) باستخدام Gradient Descent.
// */
//namespace SentimentAnalysis
//{
//    public class clsLogisticRegressionModel
//    {
//        private Dictionary<string, double> _weights = new Dictionary<string, double>();
//        private double _bias = 0;//بعض النصوص قد لا تحتوي على كلمات من القاموس
//                                 // يوفر للنموذج قيمة أساسية يمكنه البدء منها.
//        private clsFeatureExtractor _extractor;

//        public clsLogisticRegressionModel()
//        {
//            _extractor = new clsFeatureExtractor(_weights);
//        }

//        private double Sigmoid(double z) => 1.0 / (1.0 + Math.Exp(-z));

//        public void Train(clsSentimentData[] data, double learningRate, int epochs)
//        {
//            var vocabulary = new HashSet<string>();
//            foreach (var item in data)
//                vocabulary.UnionWith(item.Text.Split(' '));

//            foreach (var word in vocabulary)
//                if (!_weights.ContainsKey(word)) _weights[word] = 0;

//            for (int epoch = 0; epoch < epochs; epoch++)
//            {
//                double totalLoss = 0;
//                foreach (var item in data)
//                {
//                    var features = _extractor.ExtractFeatures(item.Text);
//                    double prediction = Sigmoid(PredictRaw(features));
//                    double error = item.Sentiment - prediction;

//                    // تحديث الأوزان
//                    for (int i = 0; i < features.Length; i++)
//                    {
//                        _weights[_weights.Keys.ToList()[i]] += learningRate * error * features[i];
//                    }
//                    _bias += learningRate * error;

//                    totalLoss += -item.Sentiment * Math.Log(prediction) - (1 - item.Sentiment) * Math.Log(1 - prediction);
//                }
//                if (epoch % 100 == 0)
//                    Console.WriteLine($"Epoch {epoch}, Loss: {totalLoss}");
//            }
//        }

//        private double PredictRaw(int[] features)
//        {
//            double sum = _bias;
//            for (int i = 0; i < features.Length; i++)
//                sum += features[i] * _weights[_weights.Keys.ToList()[i]];
//            return sum;
//        }

//        public bool Predict(string text)
//        {
//            var features = _extractor.ExtractFeatures(text);
//            double probability = Sigmoid(PredictRaw(features));
//            return probability >= 0.5;
//        }

//        public void Evaluate(clsSentimentData[] data)
//        {
//            int correct = 0;
//            int total = data.Length;

//            foreach (var item in data)
//            {
//                bool prediction = Predict(item.Text);
//                if ((prediction ? 1 : 0) == item.Sentiment)
//                    correct++;
//            }

//            double accuracy = (double)correct / total;
//            Console.WriteLine($"Accuracy: {accuracy * 100:F2}%");
//        }
//    }
//}

// ✅ خوارزمية الانحدار اللوجستي اليدوية


using SentimentAnalysis2;
using System.Collections;
public class clsLogisticRegressionModel
{
    private Dictionary<string, double> _weights = new();  // لكل كلمة وزن

    private double _bias = 0;                             // الانحياز (bias)
    private clsFeatureExtractor _extractor;               // لتحويل النص إلى ميزات


    private double Sigmoid(double z) => 1.0 / (1.0 + Math.Exp(-z)); // تحول الرقم z إلى قيمة بين 0 و 1.

    public void Train(clsSentimentData[] data, double learningRate, int epochs)
    {
        _extractor = new clsFeatureExtractor(data.Select(d => d.Text));

        foreach (var word in _extractor.Vocabulary)
            _weights[word] = 0;

        

        for (int epoch = 0; epoch < epochs; epoch++)
        {
            double totalLoss = 0;
            foreach (var item in data)
            {
                var features = _extractor.ExtractFeatures(item.Text);
                double z = _bias;
                for (int i = 0; i < features.Length; i++)
                    z += features[i] * (double)_weights[_extractor.Vocabulary[i]];


                double prediction = Sigmoid(z);
                double error = item.Sentiment - prediction;

                for (int i = 0; i < features.Length; i++)
                {
                    _weights[_extractor.Vocabulary[i]] += learningRate * error * features[i];

                }
                _bias += learningRate * error;
                totalLoss += -item.Sentiment * Math.Log(prediction) - (1 - item.Sentiment) * Math.Log(1 - prediction);
            }
            if (epoch % 100 == 0)
                Console.WriteLine($"Epoch {epoch}, Loss: {totalLoss:F4}");
        }
    }

    public bool Predict(string text)
    {
        var features = _extractor.ExtractFeatures(text);
        double z = _bias;
        for (int i = 0; i < features.Length; i++)
            z += features[i] * (double)_weights[_extractor.Vocabulary[i]];
        return Sigmoid(z) >= 0.5;
    }

    public void Evaluate(clsSentimentData[] data)
    {
        int correct = 0;
        foreach (var item in data)
        {
            bool prediction = Predict(item.Text);
            if ((prediction ? 1 : 0) == item.Sentiment)
                correct++;
        }
        double accuracy = (double)correct / data.Length;
        Console.WriteLine($"Accuracy: {accuracy * 100:F2}%");
    }
}

