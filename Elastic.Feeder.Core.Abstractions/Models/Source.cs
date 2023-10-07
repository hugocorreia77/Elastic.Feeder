using System.Text.Json.Serialization;

namespace Elastic.Feeder.Core.Abstractions.Models
{
    public class Source
    {
        [JsonIgnore]
        public string? Content { get; set; }
        public string Data { get; set; }
        public Attachment Attachment { get; set; }
    }

    public class Attachment
    {
        public string Content_type { get; set; }
        public string Content { get; set; }
        public string Format { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
    }

}
