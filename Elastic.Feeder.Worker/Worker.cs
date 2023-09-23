using Elastic.Feeder.Core.Abstractions.Observers;

namespace Elastic.Feeder.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IElasticFileObserver _observer;


        public Worker(ILogger<Worker> logger, IElasticFileObserver observer)
        {
            _logger = logger;
            _observer = observer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _observer.Observe();
        }
    }
}