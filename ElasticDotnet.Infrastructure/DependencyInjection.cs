using ElasticDotnet.Domain.Interfaces;
using ElasticDotnet.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticDotnet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        return services;
    }
}