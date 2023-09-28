namespace Elastic.Feeder.Core.Abstractions.Configurations
{
    public class ObserverConfiguration
    {
        public string Folder { get; set; }
        public string FileTypes { get; set; }
        public bool DeleteLocalFileAfterUpload { get; set; }
    }
}
