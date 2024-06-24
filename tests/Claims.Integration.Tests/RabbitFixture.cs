using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Claims.Api.Tests;
public class RabbitFixture
{
    private IContainer rabbitContainer = new ContainerBuilder()
            .WithImage("rabbitmq:3-management")
            .WithPortBinding(5673)
            .WithEnvironment("RABBITMQ_DEFAULT_USER", "guest")
            .WithEnvironment("RABBITMQ_DEFAULT_PASS", "guest")
    .Build();

    public string Host => rabbitContainer.Hostname;

    public Task InitializeAsync() => rabbitContainer.StartAsync();

    public Task DisposeAsync() => rabbitContainer.DisposeAsync().AsTask();
}
