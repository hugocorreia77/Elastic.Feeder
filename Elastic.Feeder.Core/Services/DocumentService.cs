using Elastic.Feeder.Core.Abstractions.Services;
using Elastic.Feeder.Data.Abstractions.Repository;

namespace Elastic.Feeder.Core.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IElasticRepository _elasticRepository;

        public DocumentService(IElasticRepository elasticRepository)
        {
            _elasticRepository = elasticRepository;
        }

        public async Task SaveDocument(string jsonDocument)
        {
            await _elasticRepository.WriteDocument(jsonDocument);
        }
    }
}
