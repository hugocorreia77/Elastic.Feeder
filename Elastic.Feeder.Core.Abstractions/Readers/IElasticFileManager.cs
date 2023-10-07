using Elastic.Feeder.Core.Abstractions.Models;

namespace Elastic.Feeder.Core.Abstractions.Readers
{
    public interface IElasticFileManager
    {
        FileDetails ReadFile(string path);
        bool DeleteFile(string path);
    }
}
