using DotNet.Testcontainers.Containers;
using Testcontainers.MongoDb;

namespace Claims.Api.Tests;
public class MongoFixture : IAsyncLifetime
{
    private readonly MongoDbContainer mongoContainer = new MongoDbBuilder()
            .WithImage("mongo:latest")
            .Build();

    public string Host => mongoContainer.Hostname;
    public string ConnectionString => mongoContainer.GetConnectionString();
    public string ContainerId => $"{mongoContainer.Id}";

    public Task InitializeAsync()
    {
        return mongoContainer.StartAsync();
    }

    public Task DisposeAsync() => mongoContainer.DisposeAsync().AsTask();
}
