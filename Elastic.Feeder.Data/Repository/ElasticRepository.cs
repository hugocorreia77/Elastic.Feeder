using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Feeder.Core.Abstractions.ElasticDocuments;
using Elastic.Feeder.Core.Abstractions.Models;
using Elastic.Feeder.Data.Abstractions.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nest;

namespace Elastic.Feeder.Data.Repository
{
    public class ElasticRepository : IElasticRepository
    {
        private readonly ILogger<ElasticRepository> _logger;
        private readonly ElasticClient _elasticClient;
        private readonly ElasticsearchConfiguration _elasticConfigs;

        public ElasticRepository(ILogger<ElasticRepository> logger, ElasticClient elasticClient
            , IOptions<ElasticsearchConfiguration> elasticConfigs) {
            _logger = logger;
            _elasticClient = elasticClient;
            _elasticConfigs = elasticConfigs.Value;
        }

        public async Task<bool> WriteFile(FileDetails fileDetails)
        {
            var document = new Document
            {
                Data = fileDetails.Data
            };

            var response = await _elasticClient.IndexAsync(document,
                                                            p => p.Id(fileDetails.FileName)
                                                                  .Index(_elasticConfigs.DocumentsIndex)
                                                                  .Pipeline(_elasticConfigs.DocumentsPipeline)
                                                          );

            if (response.IsValid)
            {
                _logger.LogInformation("File {document} indexed successfully!", fileDetails.FileName);
                return true;
            }

            _logger.LogError("File {document} could not be uploaded!", fileDetails.FileName);
            return false;
        }

        public async Task<IEnumerable<string>> SearchFileContentAsync(string search)
        {
            var searchResponse = await _elasticClient.SearchAsync<Source>
                    (sd => sd
                        .Index(_elasticConfigs.DocumentsIndex)
                        .Query(nq => +nq
                            .Match(m => m
                                .Field(a => a.Attachment.Content)
                                .Query(search)
                            )
                        )
                        .Source(s => s.Excludes(sf =>
                                                sf.Field(a => a.Attachment.Content)
                                                .Field(a => a.Data))
                        )
                    );

            return searchResponse.Hits.Any() ?
                        searchResponse.Hits.Select(h => h.Id)
                        : new List<string>();
        }

        public async Task<string> GetFileDataAsync(string fileName)
        {
            var searchResponse = await _elasticClient.SearchAsync<Source>
                   (sd => sd
                       .Index(_elasticConfigs.DocumentsIndex)
                       .Query(nq => +nq
                        .Ids(m => m.Values(fileName))
                       )
                       .Source(s => s.Excludes(sf =>
                                               sf.Field(a => a.Attachment.Content))
                       )
                   );

            if (searchResponse.Hits.Any())
            {
                return searchResponse.Hits.FirstOrDefault().Source.Data;
            }

            throw new ArgumentException("File not found!");
        }
    }
}