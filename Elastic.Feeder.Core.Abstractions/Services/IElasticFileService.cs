using Elastic.Feeder.Core.Abstractions.Models;

namespace Elastic.Feeder.Core.Abstractions.Services
{
    public interface IElasticFileService
    {
        Task<bool> SaveFile(FileDetails jsonDocument);
        Task<IEnumerable<string>> SearchFileContentAsync(string search);
    }
}
