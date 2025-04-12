//using System;
//using System.Collections.Generic;
//using System.Linq;

///*
// يقوم بتحويل النص إلى تمثيل رقمي (Vector) باستخدام Bag of Words.

//يأخذ كل كلمة في النص ويحولها إلى ميزة (1 أو 0) بناءً على وجودها في النموذج.

//هذا الكلاس هو المسؤول عن تحويل النصوص إلى بيانات رقمية حتى يتمكن النموذج من التعامل معها.
// */


//namespace SentimentAnalysis
//{
//    public class clsFeatureExtractor
//    {
//        private Dictionary<string, double> _weights;

//        public clsFeatureExtractor(Dictionary<string, double> weights)
//        {
//            _weights = weights;
//        }

//        public int[] ExtractFeatures(string text)
//        {
//            var words = text.Split(' ');
//            var features = new int[_weights.Count];//مصفوفة بحجم عدد الكلمات

//            foreach (var word in words)
//            {
//                if (_weights.ContainsKey(word))
//                {
//                    features[_weights.Keys.ToList().IndexOf(word)] = 1;
//                }
//            }

//            return features;
//        }
//    }
//}

public class clsFeatureExtractor
{
    private List<string> _vocabulary;

    public clsFeatureExtractor(IEnumerable<string> texts)
    {
        _vocabulary = texts.SelectMany(t => t.Split(' '))  
            .Distinct()                                    
            .ToList();                                     
    }

    public int[] ExtractFeatures(string text)
    {
        var words = text.Split(' ' , StringSplitOptions.RemoveEmptyEntries); 
        var features = new int[_vocabulary.Count];                           


        for (int i = 0; i < Vocabulary.Count; i++)
            features[i] = words.Contains(Vocabulary[i]) ? 1 : 0;  
        

        return features;
    }

    public List<string> Vocabulary => _vocabulary;
}