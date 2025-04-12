
using SentimentAnalysis2;
using System.Collections;
public class clsLogisticRegressionModel
{
    private Dictionary<string, double> _weights = new();

    private double _bias = 0;                            
    private clsFeatureExtractor _extractor;               


    private double Sigmoid(double z) => 1.0 / (1.0 + Math.Exp(-z)); 

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

