using Microsoft.Extensions.DependencyInjection;

namespace Claims.Application;
public static class DiExtensions
{
    public static IServiceCollection RegisterApplicationLayer(this IServiceCollection services)
    {
        services.AddTransient<ICoversService, CoversService>();

        return services;
    }
}
