using Claims.Domain.Claims;
using Claims.Domain.Covers;
using FluentAssertions;

namespace Claims.Domain.Tests;

public class ClaimTests
{
    [Fact]
    public void Claim_New_ForDamageCostExceedingMaxValue_Should_ReturnDomainError()
    {
        var cover = Cover.LoadFromDatabase("cover_id", DateTime.UtcNow.AddDays(-100), DateTime.UtcNow.AddDays(100), CoverType.Yacht);

        var claim = Claim.New(cover, "claim_name", ClaimType.BadWeather, 500000m);

        claim.IsSuccess.Should().BeFalse();
        claim.Error.Should().BeEquivalentTo(Claims.DomainErrors.Claims.DamageCostTooHigh);
    }

    [Fact]
    public void Claim_New_ForClaimCreatedAfterRelatedCoverPeriod_Should_ReturnDomainError()
    {
        var cover = Cover.LoadFromDatabase("cover_id", DateTime.UtcNow.AddDays(- -100), DateTime.UtcNow.AddDays(-100), CoverType.Yacht);

        var claim = Claim.New(cover, "claim_name", ClaimType.BadWeather, 400);

        claim.IsSuccess.Should().BeFalse();
        claim.Error.Should().BeEquivalentTo(Claims.DomainErrors.Claims.OutsideCoverPeriod);
    }

    [Fact]
    public void Claim_New_ForClaimCreatedBeforeRelatedCoverPeriod_Should_ReturnDomainError()
    {
        var cover = Cover.LoadFromDatabase("cover_id", DateTime.UtcNow.AddDays(100), DateTime.UtcNow.AddDays(200), CoverType.Yacht);

        var claim = Claim.New(cover, "claim_name", ClaimType.BadWeather, 400);

        claim.IsSuccess.Should().BeFalse();
        claim.Error.Should().BeEquivalentTo(Claims.DomainErrors.Claims.OutsideCoverPeriod);
    }

    [Fact]
    public void Claim_New_ForDamageCostUnderMaxLimit_And_ForClaimCreatedWithinCoverPeriod_Should_SuccessfullyCreateClaim()
    {
        var cover = Cover.LoadFromDatabase("cover_id", DateTime.UtcNow.AddDays(-100), DateTime.UtcNow.AddDays(100), CoverType.Yacht);

        var claim = Claim.New(cover, "claim_name", ClaimType.BadWeather, 400);

        claim.IsSuccess.Should().BeTrue();
        claim.Value.Name.Should().Be("claim_name");
        claim.Value.Cover.Should().BeEquivalentTo(cover);
        claim.Value.Type.Should().Be(ClaimType.BadWeather);
        claim.Value.DamageCost.Should().Be(400); ;
    }

    [Fact]
    public void Claim_LoadFromDatabase_Should_FillAllEntities()
    {
        var cover = Cover.LoadFromDatabase("cover_id", DateTime.Parse("2015-01-02"), DateTime.Parse("2016-01-02"), CoverType.Yacht);

        var claim = Claim.LoadFromDatabase("test_id", cover, DateTime.Parse("2015-03-03"), "claim_name", ClaimType.BadWeather, 1);

        claim.Id.Should().Be("test_id");
        claim.Name.Should().Be("claim_name");
        claim.Cover.Should().BeEquivalentTo(cover);
        claim.Created.Should().Be(DateTime.Parse("2015-03-03"));
        claim.Type.Should().Be(ClaimType.BadWeather);
        claim.DamageCost.Should().Be(1);
    }
}