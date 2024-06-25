using Claims.Domain.Covers;
using FluentAssertions;

namespace Claims.Domain.Tests;

public class CoverTests
{
    [Fact]
    public void Cover_New_ForStartDateInPast_Should_ReturnDomainError()
    {
        var cover = Cover.New(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(1), CoverType.Tanker);

        cover.IsSuccess.Should().BeFalse();
        cover.Error.Should().Be(DomainErrors.Covers.StartDateInPast);
    }

    [Fact]
    public void Cover_New_ForStartDateLaterThanEndDate_Should_ReturnDomainError()
    {
        var cover = Cover.New(DateTime.UtcNow.AddDays(20), DateTime.UtcNow.AddDays(10), CoverType.Tanker);

        cover.IsSuccess.Should().BeFalse();
        cover.Error.Should().Be(DomainErrors.Covers.StartDateLaterThanEndDate);
    }

    [Fact]
    public void Cover_New_ForCoverPeriodExceedingMaxValue_Should_ReturnDomainError()
    {
        var cover = Cover.New(DateTime.UtcNow.AddDays(20), DateTime.UtcNow.AddDays(2000), CoverType.Tanker);

        cover.IsSuccess.Should().BeFalse();
        cover.Error.Should().Be(DomainErrors.Covers.InsurancePeriodTooLong);
    }

    [Fact]
    public void Cover_New_ForProperCoverPeriod_Should_SuccessfullyCreateCover()
    {
        var utcNow = DateTime.UtcNow;
        var cover = Cover.New(utcNow.AddDays(20), utcNow.AddDays(60), CoverType.Tanker);

        cover.IsSuccess.Should().BeTrue();
        cover.Value.StartDate.Should().Be(utcNow.AddDays(20));
        cover.Value.EndDate.Should().Be(utcNow.AddDays(60));
        cover.Value.Type.Should().Be(CoverType.Tanker);
    }

    [Fact]
    public void Cover_New_Should_HaveProperPremiumCalculated_DependingOn_CoverType()
    {
        var utcNow = DateTime.UtcNow;
        var cover = Cover.New(utcNow.AddDays(20), utcNow.AddDays(22), CoverType.Yacht);

        cover.IsSuccess.Should().BeTrue();
        cover.Value.Premium.Value.Should().Be(4125);
    }

    [Fact]
    public void Cover_LoadFromDatabase_Should_FillAllEntities()
    {
        var cover = Cover.LoadFromDatabase("cover_id", DateTime.Parse("2015-12-12"), DateTime.Parse("2016-12-12"), CoverType.BulkCarrier);

        cover.Id.Should().Be("cover_id");
        cover.StartDate.Should().Be(DateTime.Parse("2015-12-12"));
        cover.EndDate.Should().Be(DateTime.Parse("2016-12-12"));
        cover.Type.Should().Be(CoverType.BulkCarrier);
    }
}
