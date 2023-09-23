namespace Elastic.Feeder.Core.Abstractions.Converters
{
    public interface IElasticFileConverter
    {

        string FromXmlToJson(string xmlString);

    }
}
