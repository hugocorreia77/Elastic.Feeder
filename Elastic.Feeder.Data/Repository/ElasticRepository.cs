using Elastic.Clients.Elasticsearch;
using Elastic.Feeder.Core.Abstractions.ElasticDocuments;
using Elastic.Feeder.Data.Abstractions.Repository;

namespace Elastic.Feeder.Data.Repository
{
    public class ElasticRepository : IElasticRepository
    {
        private readonly ElasticsearchClient _elasticClient;

        private readonly string MYINDEX = "mydocuments";

        public ElasticRepository(ElasticsearchClient elasticClient) {
            _elasticClient = elasticClient;
        }

        public async Task WriteDocument(string jsonDocument)
        {
            var document = new Document
            {
                data = jsonDocument
            };

            var response = await _elasticClient.IndexAsync<Document>(document, MYINDEX, i => i.Pipeline("documents-pipeline").Id("sample"));


        }
    }
}