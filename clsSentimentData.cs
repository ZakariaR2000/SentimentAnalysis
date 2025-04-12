using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 كلاس يحتوي على البيانات (Features) التي سيعمل عليها النموذج.

يحتوي على خصائص (Text و Sentiment) لتعريف نص التعليق والتصنيف (1 للإيجابي، 0 للسلبي).

يساعد هذا الكلاس في تنظيم البيانات التي ستدخل في النموذج.
 */

namespace SentimentAnalysis2
{
    public class clsSentimentData
    {
        public string Text { get; set; }
        public int Sentiment { get; set; }
    }
}
