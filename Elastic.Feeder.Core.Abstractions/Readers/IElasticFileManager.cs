using Elastic.Feeder.Core.Abstractions.Models;

namespace Elastic.Feeder.Core.Abstractions.Readers
{
    public interface IElasticFileManager
    {
        Task<FileDetails> ReadFileAsync(string path);
        bool DeleteFile(string path);
    }
}
