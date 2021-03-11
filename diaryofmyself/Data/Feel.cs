using System;
namespace diaryofmyself.Data.Feel
{
    public enum Emotion
    {
        Bad,
        Normal,
        Good,
    }
    public class Feeling : Base
    { 
        public virtual Emotion emotion { get; set; }
        public virtual string userUID { get; set; }

        public Feeling(Emotion emotionP, string userUIDP)
        {
            emotion = emotionP;
            userUID = userUIDP;
        }
    }
}