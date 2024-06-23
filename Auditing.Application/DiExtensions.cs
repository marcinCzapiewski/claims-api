using Auditing.Persistance;
using Claims.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auditing.Application;
public static class DiExtensions
{
    public static IServiceCollection AddAuditing(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterPersistance(configuration);
        services.AddTransient<IAuditer, Auditer>();

        return services;
    }
}
