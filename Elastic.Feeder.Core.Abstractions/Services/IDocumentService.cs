using Elastic.Feeder.Core.Abstractions.Models;

namespace Elastic.Feeder.Core.Abstractions.Services
{
    public interface IDocumentService
    {
        Task<bool> SaveDocument(FileDetails jsonDocument);
    }
}
