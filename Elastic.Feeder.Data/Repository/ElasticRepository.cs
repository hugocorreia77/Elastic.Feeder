using Elastic.Clients.Elasticsearch;
using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Feeder.Core.Abstractions.ElasticDocuments;
using Elastic.Feeder.Core.Abstractions.Models;
using Elastic.Feeder.Data.Abstractions.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Elastic.Feeder.Data.Repository
{
    public class ElasticRepository : IElasticRepository
    {
        private readonly ILogger<ElasticRepository> _logger;
        private readonly ElasticsearchClient _elasticClient;
        private readonly ElasticsearchConfiguration _elasticConfigs;

        private readonly string MYINDEX = "mydocuments";

        public ElasticRepository(ILogger<ElasticRepository> logger, ElasticsearchClient elasticClient
            , IOptions<ElasticsearchConfiguration> elasticConfigs) {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticConfigs = elasticConfigs.Value;
        }

        public async Task<bool> WriteDocument(FileDetails fileDetails)
        {
            var document = new Document
            {
                data = fileDetails.Data
            };

            var response = await _elasticClient.IndexAsync(document, _elasticConfigs.DocumentsIndex,
                                                        i => i.Pipeline(_elasticConfigs.DocumentsPipeline)
                                                                    .Id(fileDetails.FileName));

            if(response.IsSuccess())
            {
                _logger.LogInformation("File {document} indexed successfully!", fileDetails.FileName);
                return true;
            }

            _logger.LogError("File {document} could not be uploaded!", fileDetails.FileName);
            return false;
        }
    }
}