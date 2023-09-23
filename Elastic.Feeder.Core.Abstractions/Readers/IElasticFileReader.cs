namespace Elastic.Feeder.Core.Abstractions.Readers
{
    public interface IElasticFileReader
    {
        Task<string> ReadFileAsync(string path);
    }
}
