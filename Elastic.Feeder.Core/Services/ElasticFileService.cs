using Elastic.Feeder.Core.Abstractions.Models;
using Elastic.Feeder.Core.Abstractions.Services;
using Elastic.Feeder.Data.Abstractions.Repository;
using System.Text.Json;

namespace Elastic.Feeder.Core.Services
{
    public class ElasticFileService : IElasticFileService
    {
        private readonly IElasticRepository _elasticRepository;

        public ElasticFileService(IElasticRepository elasticRepository)
        {
            _elasticRepository = elasticRepository;
        }

        public Task<string> GetFileDataAsync(string fileName)
            => _elasticRepository.GetFileDataAsync(fileName);


        public Task<bool> SaveFile(FileDetails jsonDocument)
            => _elasticRepository.WriteFile(jsonDocument);

        public Task<IEnumerable<string>> SearchFileContentAsync(string search)
            => _elasticRepository.SearchFileContentAsync(search);

    }
}
