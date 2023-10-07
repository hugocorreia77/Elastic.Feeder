using Elastic.Feeder.Core.Abstractions.Configurations;
using Nest;

namespace Elastic.Feeder.WebApi.Bootstrap
{
    public static class BootstrapExtensions
    {
        public static IServiceCollection AddElasticSearchClient(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticConfig = new ElasticsearchConfiguration();
            configuration.GetSection(nameof(ElasticsearchConfiguration)).Bind(elasticConfig);

            var settings = new ConnectionSettings(new Uri(elasticConfig.Url));

            services.AddSingleton<ElasticClient>(c => new ElasticClient(settings));

            return services;
        }
    }
}
