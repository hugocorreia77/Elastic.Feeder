using Elastic.Feeder.Core.Abstractions.Converters;
using Elastic.Feeder.Core.Abstractions.Readers;
using Elastic.Feeder.Core.Observers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Xml;

namespace Elastic.Feeder.Core.Readers
{
    public class ElasticFileReaderXml : ElasticFileReader, IElasticFileReader
    {
        private readonly ILogger<ElasticFileObserver> _logger;
        private readonly IElasticFileConverter _fileConverter;

        public ElasticFileReaderXml(ILogger<ElasticFileObserver> logger, IElasticFileConverter fileConverter) : base(logger)
        {
            _logger = logger;
            _fileConverter = fileConverter;
        }

        public override async Task<string> ReadFileAsync(string path)
        {
            _logger.LogInformation($"Reading XML file : {path}");

            var xml = string.Empty;
            try
            {
                using StreamReader reader = new(path);
                xml = await reader.ReadToEndAsync();
                _logger.LogInformation($"XML File Content: \n {xml}");
            }
            catch
            {
                _logger.LogError($"An error occured while reading file: {path}");
                throw;
            }

            return GetJsonDataFromXml(xml);
            
;       }

        private string GetJsonDataFromXml(string xml)
        {
            var json = _fileConverter.FromXmlToJson(xml);
            _logger.LogInformation($"JSON from XML: {json}");
            return json;
        }

    }
}
