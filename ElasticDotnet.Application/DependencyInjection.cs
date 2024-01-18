using System.Text;
using ElasticDotnet.Application.Validators;
using ElasticDotnet.Domain.Config;
using ElasticDotnet.Domain.Models;
using Elasticsearch.Net;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace ElasticDotnet.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, string elasticCloudId, string elasticPassword)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped<IValidator<AuthDTO>, AuthValidator>();

        services.AddSingleton<IElasticClient>(_ =>
        {
            var settings = new ConnectionSettings(
                new CloudConnectionPool(elasticCloudId,
                new BasicAuthenticationCredentials("elastic", elasticPassword))
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

            return new ElasticClient(settings);
        });


        return services;
    }
}