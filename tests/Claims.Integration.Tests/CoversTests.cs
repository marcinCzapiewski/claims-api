using Claims.Api.Covers;
using Claims.Api.Tests;
using Claims.Domain.Covers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Claims.Integration.Tests;
public class CoversTests(MongoFixture mongoFixture) : IClassFixture<MongoFixture>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory = WebAppFactory.NewInstance(mongoFixture);

    [Fact]
    public async Task POST_Covers_For_Valid_Input_Should_Return_201_Status_Code()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/covers", new StringContent(JsonConvert.SerializeObject(new CreateCoverRequest
        {
            Type = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10)
        }), Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task POST_Covers_For_Invalid_Input_Should_Return_400_Status_Code()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsync("/covers", new StringContent(JsonConvert.SerializeObject(new CreateCoverRequest
        {
            Type = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow.AddDays(100),
            EndDate = DateTime.UtcNow.AddDays(10)
        }), Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GET_Covers_Should_Return_200_Status_Code_And_Contain_Covers()
    {
        var client = _factory.CreateClient();

        await client.PostAsync("/covers", new StringContent(JsonConvert.SerializeObject(new CreateCoverRequest
        {
            Type = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10)
        }), Encoding.UTF8, "application/json"));

        var response = await client.GetAsync("/covers");

        var coversJson = await response.Content.ReadAsStringAsync();
        var covers = JsonConvert.DeserializeObject<IEnumerable<CoverReadModel>>(coversJson);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        covers!.Count().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GET_Cover_Should_Return_200_Status_Code_And_Cover_If_Exists()
    {
        var client = _factory.CreateClient();

        var addedCoverResponse = await client.PostAsync("/covers", new StringContent(JsonConvert.SerializeObject(new CreateCoverRequest
        {
            Type = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow.AddDays(1).Date,
            EndDate = DateTime.UtcNow.AddDays(10).Date
        }), Encoding.UTF8, "application/json"));

        var addedCoverJson = await addedCoverResponse.Content.ReadAsStringAsync();
        var addedCover = JsonConvert.DeserializeObject<CoverReadModel>(addedCoverJson);

        var response = await client.GetAsync($"/covers/{addedCover!.Id}");

        var coverJson = await response.Content.ReadAsStringAsync();
        var cover = JsonConvert.DeserializeObject<CoverReadModel>(coverJson);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        cover.Should().BeEquivalentTo(addedCover);
    }

    [Fact]
    public async Task DELETE_Cover_Should_Return_200_Status_Code_And_Cover_If_Exists()
    {
        var client = _factory.CreateClient();

        var addedCoverResponse = await client.PostAsync("/covers", new StringContent(JsonConvert.SerializeObject(new CreateCoverRequest
        {
            Type = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10)
        }), Encoding.UTF8, "application/json"));

        var coversJson = await addedCoverResponse.Content.ReadAsStringAsync();
        var addedCover = JsonConvert.DeserializeObject<CoverReadModel>(coversJson);

        var response = await client.DeleteAsync($"/covers/{addedCover!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    public async Task DisposeAsync()
    {
        var mongoClient = new MongoClient(mongoFixture.ConnectionString);
        var database = mongoClient.GetDatabase("Claims");

        await database.DropCollectionAsync("covers");
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}
