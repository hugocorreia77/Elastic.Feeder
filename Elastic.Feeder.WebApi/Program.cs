using Elastic.Feeder.Core.Abstractions.Configurations;
using Elastic.Feeder.Core.Abstractions.Services;
using Elastic.Feeder.Core.Services;
using Elastic.Feeder.Data.Abstractions.Repository;
using Elastic.Feeder.Data.Repository;
using Elastic.Feeder.WebApi.Bootstrap;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ElasticsearchConfiguration>(builder.Configuration.GetSection(nameof(ElasticsearchConfiguration)));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IElasticRepository, ElasticRepository>();
builder.Services.AddTransient<IElasticFileService, ElasticFileService>();

BootstrapExtensions.AddElasticSearchClient(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
