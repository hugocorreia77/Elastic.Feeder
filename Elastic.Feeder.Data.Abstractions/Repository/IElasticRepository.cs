using Elastic.Feeder.Core.Abstractions.Models;

namespace Elastic.Feeder.Data.Abstractions.Repository
{
    public interface IElasticRepository
    {
        Task<bool> WriteFile(FileDetails fileDetails);
        Task<IEnumerable<string>> SearchFileContentAsync(string search);
        Task<string> GetFileDataAsync(string fileName);
    }
}
