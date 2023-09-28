namespace Elastic.Feeder.Core.Abstractions.Services
{
    public interface IDocumentService
    {
        Task SaveDocument(string jsonDocument);
    }
}
