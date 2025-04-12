

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
