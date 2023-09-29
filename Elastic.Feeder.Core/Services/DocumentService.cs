using Elastic.Feeder.Core.Abstractions.Models;
using Elastic.Feeder.Core.Abstractions.Services;
using Elastic.Feeder.Data.Abstractions.Repository;
using System.Text.Json;

namespace Elastic.Feeder.Core.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IElasticRepository _elasticRepository;

        public DocumentService(IElasticRepository elasticRepository)
        {
            _elasticRepository = elasticRepository;
        }

        public Task<bool> SaveDocument(FileDetails jsonDocument)
            => _elasticRepository.WriteDocument(jsonDocument);

        public Task<IEnumerable<string>> Search(string search)
            => _elasticRepository.Search(search);

    }
}
