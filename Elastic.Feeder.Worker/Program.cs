using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Feeder.Core.Abstractions.Converters;
using Elastic.Feeder.Core.Abstractions.Observers;
using Elastic.Feeder.Core.Converters;
using Elastic.Feeder.Core.Observers;
using Elastic.Feeder.Core.Readers;
using Elastic.Feeder.Worker;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        services.Configure<ObserverConfiguration>(configuration.GetSection(nameof(ObserverConfiguration)));

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


        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
