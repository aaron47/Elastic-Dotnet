using System.Text;
using elastic_dotnet.Config;
using elastic_dotnet.Models;
using elastic_dotnet.Services;
using Elasticsearch.Net;
using Nest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IElasticClient>(services =>
{
    var elasticConfig = builder.Configuration.GetSection("Elastic").Get<ElasticConfiguration>() ?? throw new InvalidOperationException("Elasticsearch configuration is missing.");
    var settings = new ConnectionSettings(
        new CloudConnectionPool(elasticConfig.CloudId,
        new BasicAuthenticationCredentials(elasticConfig.Username, elasticConfig.Password))
    )
    .DisableDirectStreaming()
    .DefaultMappingFor<Product>(m => m
        .PropertyName(p => p.ProductName, "ProductName")
        .PropertyName(p => p.Description, "Description")
    )
    .OnRequestCompleted(response =>
        {
            Console.WriteLine($"Request: {Encoding.UTF8.GetString(response.RequestBodyInBytes)}");
            Console.WriteLine($"Response: {Encoding.UTF8.GetString(response.ResponseBodyInBytes)}");
        });
    ;

    return new ElasticClient(settings);
});
builder.Services.AddHttpClient<ISentenceEncoder, SentenceEncoder>();
builder.Services.Configure<PythonMicroserviceOptions>(builder.Configuration.GetSection("PythonMicroservice"));


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
