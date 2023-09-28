using Elastic.Feeder.Core.Abstractions.Models;

namespace Elastic.Feeder.Data.Abstractions.Repository
{
    public interface IElasticRepository
    {

        Task<bool> WriteDocument(FileDetails fileDetails);

    }
}
