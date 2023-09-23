namespace Elastic.Feeder.Core.Abstractions.Observers
{
    public interface IElasticFileObserver
    {
        Task Observe();
    }
}
