namespace Elastic.Feeder.Data.Abstractions.Repository
{
    public interface IElasticRepository
    {

        Task WriteDocument(string jsonDocument);

    }
}
