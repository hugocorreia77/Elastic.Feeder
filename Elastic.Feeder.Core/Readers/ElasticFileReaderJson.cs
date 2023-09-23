using Elastic.Feeder.Core.Abstractions.Readers;
using Elastic.Feeder.Core.Observers;
using Microsoft.Extensions.Logging;

namespace Elastic.Feeder.Core.Readers
{
    public class ElasticFileReaderJson : ElasticFileReader, IElasticFileReader
    {
        private readonly ILogger<ElasticFileObserver> _logger;

        public ElasticFileReaderJson(ILogger<ElasticFileObserver> logger) : base(logger)
        {
            _logger = logger;
        }

        public override Task ReadFile(string path)
        {
            _logger.LogInformation($"Reading JSON file : {path}");

            using StreamReader reader = new(path);
            var json = reader.ReadToEnd();

            _logger.LogInformation("JSON File Content:");
            _logger.LogInformation($"{json}");

            return Task.CompletedTask;
        }
    }
}
