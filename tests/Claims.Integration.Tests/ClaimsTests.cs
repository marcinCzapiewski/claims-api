using Claims.Api.Claims;
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
public class ClaimsTests(MongoFixture mongoFixture) : IClassFixture<MongoFixture>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _factory = WebAppFactory.NewInstance(mongoFixture);

    // normally at least 2 tests per endpoint (successful and not) should be implemented
    // here are just some example tests

    [Fact]
    public async Task POST_Claims_For_Invalid_Input_Should_Return_400_Status_Code()
    {
        var client = _factory.CreateClient();

        var addedCoverResponse = await client.PostAsync("/covers", new StringContent(JsonConvert.SerializeObject(new CreateCoverRequest
        {
            Type = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow.AddDays(3),
            EndDate = DateTime.UtcNow.AddDays(10)
        }), Encoding.UTF8, "application/json"));

        var addedCoverJson = await addedCoverResponse.Content.ReadAsStringAsync();
        var relatedCover = JsonConvert.DeserializeObject<CoverReadModel>(addedCoverJson);

        var response = await client.PostAsync("/claims", new StringContent(JsonConvert.SerializeObject(new CreateClaimRequest
        {
            CoverId = relatedCover!.Id,
            Name = "claim1",
            DamageCost = 1,
            Type = Domain.Claims.ClaimType.Grounding
        }), Encoding.UTF8, "application/json"));

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GET_Claims_Should_Return_200_Status_Code_And_Contain_Covers()
    {
        var client = _factory.CreateClient();

        await client.PostAsync("/claims", new StringContent(JsonConvert.SerializeObject(new CreateCoverRequest
        {
            Type = CoverType.BulkCarrier,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(10)
        }), Encoding.UTF8, "application/json"));

        var response = await client.GetAsync("/covers");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
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
