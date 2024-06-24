using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Claims.Api.Tests;
public class MongoFixture : IAsyncLifetime
{
    private IContainer mongoContainer = new ContainerBuilder()
            .WithImage("mongo:latest")
            .WithPortBinding(27018)
    .Build();

    public string Host => mongoContainer.Hostname;

    public Task InitializeAsync() => mongoContainer.StartAsync();

    public Task DisposeAsync() => mongoContainer.DisposeAsync().AsTask();
}
