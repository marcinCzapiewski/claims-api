using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace Claims.Application;
public static class DiExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();

            config.NotificationPublisher = new TaskWhenAllPublisher();
        });

        return services;
    }
}
