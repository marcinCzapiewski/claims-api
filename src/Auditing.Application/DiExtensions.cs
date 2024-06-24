using Auditing.Application.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auditing.Application;
public static class DiExtensions
{
    public static IServiceCollection AddAuditing(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuditContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumer<ClaimCreatedConsumer>();
            config.AddConsumer<ClaimDeletedonsumer>();
            config.AddConsumer<CoverCreatedConsumer>();
            config.AddConsumer<CoverDeletedConsumer>();

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
                {
                    h.Username(configuration["MessageBroker:Username"]!);
                    h.Password(configuration["MessageBroker.Password"]!);
                });

                configurator.ConfigureEndpoints(context);
            });
        });

        services.AddTransient<AuditService>();

        return services;
    }
}
