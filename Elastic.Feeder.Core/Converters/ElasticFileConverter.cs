using Elastic.Feeder.Core.Abstractions.Converters;
using Elastic.Feeder.Core.Observers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Xml;

namespace Elastic.Feeder.Core.Converters
{
    public class ElasticFileConverter : IElasticFileConverter
    {
        private readonly ILogger<ElasticFileObserver> _logger;

        public ElasticFileConverter(ILogger<ElasticFileObserver> logger) 
        {
            _logger = logger;
        }

        public string FromXmlToJson(string xmlString)
        {
            _logger.LogInformation($"Converting XML to Json...");

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);

                _logger.LogInformation($"Json result : {json}");
                return json;
            }
            catch {
                _logger.LogError($"It was not possible to convert the XML file to JSON!");
                throw;
            }

        }
    }
}
