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

        public override async Task<string> ReadFileAsync(string path)
        {
            _logger.LogInformation($"Reading JSON file : {path}");
            try
            {
                using StreamReader reader = new(path);
                var json = await reader.ReadToEndAsync();

                _logger.LogInformation("JSON File Content:");
                _logger.LogInformation($"{json}");

                return json;
            }
            catch
            {
                _logger.LogError($"An error occured while reading file: {path}");
                throw;
            }
            
        }
    }
}
