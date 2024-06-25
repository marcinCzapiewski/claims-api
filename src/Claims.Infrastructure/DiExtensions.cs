using Claims.Domain.Claims;
using Claims.Domain.Covers;
using Claims.Infrastructure.Database;
using Claims.Infrastructure.Database.Claims;
using Claims.Infrastructure.Database.Covers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Claims.Infrastructure;
public static class DiExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
                var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
                options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
            }
        );

        services.AddTransient<ICoversRepository, CoversRepository>();
        services.AddTransient<IClaimsRepository, ClaimsRepository>();

        //services.AddMassTransit(config =>
        //{
        //    config.SetKebabCaseEndpointNameFormatter();

        //    config.UsingRabbitMq((context, configurator) =>
        //    {
        //        configurator.Host(new Uri(configuration["MessageBroker:Host"]!), h =>
        //        {
        //            h.Username(configuration["MessageBroker:Username"]!);
        //            h.Password(configuration["MessageBroker.Password"]!);
        //        });

        //        configurator.ConfigureEndpoints(context);
        //    });
        //});

        return services;
    }
}
