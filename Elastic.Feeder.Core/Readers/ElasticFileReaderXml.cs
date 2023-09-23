using Elastic.Feeder.Core.Abstractions.Readers;
using Elastic.Feeder.Core.Observers;
using Microsoft.Extensions.Logging;

namespace Elastic.Feeder.Core.Readers
{
    public class ElasticFileReaderXml : ElasticFileReader, IElasticFileReader
    {
        private readonly ILogger<ElasticFileObserver> _logger;

        public ElasticFileReaderXml(ILogger<ElasticFileObserver> logger) : base(logger)
        {
            _logger = logger;
        }

        public override Task ReadFile(string path)
        {
            _logger.LogInformation($"Reading XML file : {path}");

            using StreamReader reader = new(path);
            var xml = reader.ReadToEnd();

            _logger.LogInformation("XML File Content:");
            _logger.LogInformation($"{xml}");


            return Task.CompletedTask;
        }
    }
}
