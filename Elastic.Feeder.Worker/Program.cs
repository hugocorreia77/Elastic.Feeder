using Elastic.Clients.Elasticsearch;
using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Feeder.Core.Abstractions.Converters;
using Elastic.Feeder.Core.Abstractions.Observers;
using Elastic.Feeder.Core.Abstractions.Services;
using Elastic.Feeder.Core.Converters;
using Elastic.Feeder.Core.Observers;
using Elastic.Feeder.Core.Readers;
using Elastic.Feeder.Core.Services;
using Elastic.Feeder.Data.Abstractions.Repository;
using Elastic.Feeder.Data.Repository;
using Elastic.Feeder.Worker;
using Elastic.Feeder.Worker.Boostrap;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.Configure<ObserverConfiguration>(configuration.GetSection(nameof(ObserverConfiguration)));
        //services.Configure<ElasticsearchConfiguration>(configuration.GetSection(nameof(ElasticsearchConfiguration)));



        services.AddTransient<ElasticFileReaderJson>();
        services.AddTransient<ElasticFileReaderXml>();

        services.AddTransient<ElasticFileReaderResolver>(serviceProvider => key =>
        {
            switch (key.ToUpper())
            {
                case "JSON":
                    return serviceProvider.GetService<ElasticFileReaderJson>();
                case "XML":
                    return serviceProvider.GetService<ElasticFileReaderXml>();
                default:
                    throw new KeyNotFoundException(); // or maybe return null, up to you
            }
        }); 

        services.AddTransient<IElasticFileConverter, ElasticFileConverter>();
        services.AddSingleton<IElasticFileObserver, ElasticFileObserver>();

        services.AddTransient<IElasticRepository, ElasticRepository>();
        services.AddTransient<IDocumentService, DocumentService>();

        services.AddElasticSearchClient(configuration);

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
