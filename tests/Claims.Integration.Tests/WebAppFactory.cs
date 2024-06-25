using Claims.Api.Tests;
using Claims.Infrastructure.Database;
using MassTransit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Claims.Integration.Tests;
internal static class WebAppFactory
{
    public static WebApplicationFactory<Program> NewInstance(MongoFixture mongoFixture) => new WebApplicationFactory<Program>()
        .WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var appDbContext = services.First(descriptor => descriptor.ServiceType == typeof(ApplicationDbContext));
                services.Remove(appDbContext);

                var contextOptions = services.First(descriptor => descriptor.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                services.Remove(contextOptions);

                services.AddDbContext<ApplicationDbContext>(
                options =>
                {
                    var client = new MongoClient(mongoFixture.ConnectionString);
                    var database = client.GetDatabase("Claims");
                    options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
                });

                services.AddMassTransitTestHarness();
            });
        });
}
