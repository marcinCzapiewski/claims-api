using Claims.Application;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MassTransit;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Claims.Api.Contracts.Requests;
using System.Text;
using FluentAssertions;
using System.Net;
using Claims.Application.Covers;

namespace Claims.Api.Tests;

public class CoversTests(MongoFixture mongoFixture, RabbitFixture rabbitFixture) : IClassFixture<MongoFixture>, IClassFixture<RabbitFixture>, IDisposable
{
    private IContainer mongoContainer;
    private IContainer rabbitContainer;

    private WebApplicationFactory<Program> _factory;

    public async Task DisposeAsync()
    {
        await mongoContainer.StopAsync();
        await mongoContainer.DisposeAsync();

        await rabbitContainer.StopAsync();
        await rabbitContainer.DisposeAsync();
    }

    // TODO other integrations tests for basic add/delete/get/list flows for covers and claims

    [Fact]
    public async Task AddCover()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddDbContext<ClaimsContext>(
                        options =>
                        {
                            var client = new MongoClient($"mongodb://{mongoFixture.Host}:27018");
                            var database = client.GetDatabase("Claims");
                            options.UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName);
                        }
                    );

                    services.RemoveMassTransitHostedService();
                    services.AddMassTransitTestHarness(config =>
                    {
                        config.SetKebabCaseEndpointNameFormatter();

                        config.UsingRabbitMq((context, configurator) =>
                        {
                            configurator.Host(new Uri($"rabbitmq://guest:guest@{rabbitFixture.Host}:5673"), h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            configurator.ConfigureEndpoints(context);
                        });
                    });
                });
            });

        var client = _factory.CreateClient();

        var utcNow = DateTime.UtcNow;
        var response = await client.PostAsync(
            "/covers",
            new StringContent(JsonConvert.SerializeObject(new CreateCoverRequest
            {
                StartDate = utcNow.AddDays(1),
                EndDate = utcNow.AddDays(10),
                Type = Contracts.CoverType.Yacht
            }), Encoding.UTF8, "application/json"));

        var content = response.Content.ReadFromJsonAsync<CoverDto>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        content.Should().BeEquivalentTo(new CoverDto
        {
            StartDate = utcNow.AddDays(1),
            EndDate = utcNow.AddDays(10),
            Type = CoverType.Yacht
        });
    }

    public void Dispose()
    {
        
    }
}
