using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Feeder.Core.Abstractions.Converters;
using Elastic.Feeder.Core.Abstractions.Observers;
using Elastic.Feeder.Core.Abstractions.Readers;
using Elastic.Feeder.Core.Abstractions.Services;
using Elastic.Feeder.Core.Converters;
using Elastic.Feeder.Core.FileManagers;
using Elastic.Feeder.Core.Observers;
using Elastic.Feeder.Core.Services;
using Elastic.Feeder.Data.Abstractions.Repository;
using Elastic.Feeder.Data.Repository;
using Elastic.Feeder.Worker;
using Elastic.Feeder.Worker.Boostrap;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.Configure<ObserverConfiguration>(configuration.GetSection(nameof(ObserverConfiguration)));
        services.Configure<ElasticsearchConfiguration>(configuration.GetSection(nameof(ElasticsearchConfiguration)));
        
        services.AddTransient<IElasticFileManager, ElasticFileManager>();

        services.AddTransient<IElasticFileConverter, ElasticFileConverter>();
        services.AddSingleton<IElasticFileObserver, ElasticFileObserver>();

        services.AddTransient<IElasticRepository, ElasticRepository>();
        services.AddTransient<IElasticFileService, ElasticFileService>();

        services.AddElasticSearchClient(configuration);

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
