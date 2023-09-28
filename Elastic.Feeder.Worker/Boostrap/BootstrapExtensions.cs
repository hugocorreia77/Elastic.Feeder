using Elastic.Clients.Elasticsearch;
using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Elastic.Feeder.Worker.Boostrap
{
    public static class BootstrapExtensions
    {
        public static IServiceCollection AddElasticSearchClient(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticConfig = new ElasticsearchConfiguration();
            configuration.GetSection(nameof(ElasticsearchConfiguration)).Bind(elasticConfig);

            var elasticSearchSettings = new ElasticsearchClientSettings(new Uri(elasticConfig.Url))
                                .CertificateFingerprint(elasticConfig.Fingerprint)
                                .Authentication(new BasicAuthentication(elasticConfig.Username, elasticConfig.Password));



            services.AddSingleton<ElasticsearchClient>(x => new ElasticsearchClient(elasticSearchSettings));

            return services;
        }
    }
}
