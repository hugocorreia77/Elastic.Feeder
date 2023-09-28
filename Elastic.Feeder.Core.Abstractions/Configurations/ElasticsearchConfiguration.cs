namespace Elastic.Feeder.Core.Abstractions.Configurations
{
    public class ElasticsearchConfiguration
    {
        public string Url { get; set; }
        public string Fingerprint { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
