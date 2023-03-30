using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    public class DetectedLanguage
    {
        [JsonProperty("language")]
        public string language { get; set; }

        [JsonProperty("score")]
        public double score { get; set; }
    }

    public class Root
    {
        [JsonProperty("detectedLanguage")]
        public DetectedLanguage detectedLanguage { get; set; }

        [JsonProperty("translations")]
        public List<Translation> translations { get; set; }

    }

    public class Translation
    {
        [JsonProperty("text")]
        public string text { get; set; }

        [JsonProperty("to")]
        public string to { get; set; }
    }

}
