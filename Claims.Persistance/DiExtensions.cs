using Claims.Domain;
using Claims.Domain.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Claims.Persistance;
public static class DiExtensions
{
    public static IServiceCollection RegisterPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ClaimsContext>(
            options =>
            {
                var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
                var database = client.GetDatabase(configuration["MongoDb:DatabaseName"]);
                options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
            }
        );

        services.AddTransient<ICoversRepository, CoversRepository>();
        services.AddTransient<IClaimsRepository, ClaimsRepository>();

        return services;
    }
}
