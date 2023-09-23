using Elastic.Feeder.Core.Abstractions.Readers;

namespace Elastic.Feeder.Core.Readers
{
    public delegate IElasticFileReader ElasticFileReaderResolver(string key);

}
