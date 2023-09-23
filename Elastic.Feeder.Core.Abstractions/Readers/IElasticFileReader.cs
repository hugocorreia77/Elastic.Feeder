namespace Elastic.Feeder.Core.Abstractions.Readers
{
    public interface IElasticFileReader
    {
        Task ReadFile(string path);
    }
}
