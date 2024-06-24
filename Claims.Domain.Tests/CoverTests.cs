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
        cover.Value.Premium.Should().Be(4125);
    }

    [Theory]
    [InlineData(CoverType.BulkCarrier, 1, 1625)]
    [InlineData(CoverType.ContainerShip, 1, 1625)]
    [InlineData(CoverType.Yacht, 1, 1375)]
    [InlineData(CoverType.PassengerShip, 1, 1500)]
    [InlineData(CoverType.Tanker, 1, 1875)]
    [InlineData(CoverType.BulkCarrier, 30, 1625 * 30)]
    [InlineData(CoverType.ContainerShip, 30, 1625 * 30)]
    [InlineData(CoverType.Yacht, 30, 1375 * 30)]
    [InlineData(CoverType.PassengerShip, 30, 1500 * 30)]
    [InlineData(CoverType.Tanker, 30, 1875 * 30)]
    public void Cover_ComputePremium_ForCoverShorterOrEqualTo30Days_Should_CalculatePremiumBasedOnPeriodLengthAndType(CoverType coverType, int coverDays, decimal expectedPremium)
    {
        var tomorrow = DateTime.UtcNow.AddDays(1);
        var startDate = tomorrow;
        var endDate = tomorrow.AddDays(coverDays - 1);

        var premium = Cover.ComputePremium(startDate, endDate, coverType);

        premium.Should().Be(expectedPremium);
    }

    [Theory]
    [InlineData(CoverType.Yacht, 31, 30 * 1375 + 1 * (1375 * 0.95))]
    [InlineData(CoverType.BulkCarrier, 31, 30 * 1625 + 1 * (1625 * 0.98))]
    [InlineData(CoverType.ContainerShip, 31, 30 *  1625 + 1 * (1625 * 0.98))]
    [InlineData(CoverType.PassengerShip, 31, 30 * 1500 + 1 * (1500 * 0.98))]
    [InlineData(CoverType.Tanker, 31, 30 *  1875 + 1 * (1875 * 0.98))]
    [InlineData(CoverType.Yacht, 180, 30 * 1375 + 150 * (1375 * 0.95))]
    [InlineData(CoverType.BulkCarrier, 180, 30 * 1625 + 150 * (1625 * 0.98))]
    [InlineData(CoverType.ContainerShip, 180, 30 * 1625 + 150 * (1625 * 0.98))]
    [InlineData(CoverType.PassengerShip, 180, 30 * 1500 + 150 * (1500 * 0.98))]
    [InlineData(CoverType.Tanker, 180, 30 * 1875 + 150 * (1875 * 0.98))]
    public void Cover_ComputePremium_ForCoverLongerThan30DaysAndShorterOrEqual180ThanDays_Should_CalculatePremiumBasedOnPeriodLengthAndType_AndApplyDiscountAfter30Days(CoverType coverType, int coverDays, decimal expectedPremium)
    {
        var tomorrow = DateTime.UtcNow.AddDays(1);
        var startDate = tomorrow;
        var endDate = tomorrow.AddDays(coverDays - 1);

        var premium = Cover.ComputePremium(startDate, endDate, coverType);

        premium.Should().Be(expectedPremium);
    }

    [Theory]
    [InlineData(CoverType.Yacht, 181, 30 * 1375 + 150 * (1375 * 0.95) + 1 * (1375 * 0.95 * 0.97))]
    [InlineData(CoverType.BulkCarrier, 181, 30 * 1625 + 150 * (1625 * 0.98) + 1 * (1625 * 0.98 * 0.99))]
    [InlineData(CoverType.ContainerShip, 181, 30 * 1625 + 150 * (1625 * 0.98) + 1 * (1625 * 0.98 * 0.99))]
    [InlineData(CoverType.PassengerShip, 181, 30 * 1500 + 150 * (1500 * 0.98) + 1 * (1500 * 0.98 * 0.99))]
    [InlineData(CoverType.Tanker, 181, 30 * 1875 + 150 * (1875 * 0.98) + 1 * (1875 * 0.98 * 0.99))]
    public void Cover_ComputePremium_ForCoverLongerThan180Days_Should_CalculatePremiumBasedOnPeriodLengthAndType_AndApplyDiscountProgressively(CoverType coverType, int coverDays, decimal expectedPremium)
    {
        var tomorrow = DateTime.UtcNow.AddDays(1);
        var startDate = tomorrow;
        var endDate = tomorrow.AddDays(coverDays - 1);

        var premium = Cover.ComputePremium(startDate, endDate, coverType);

        premium.Should().Be(expectedPremium);
    }

    [Fact]
    public void Cover_LoadFromDatabase_Should_FillAllEntities()
    {
        var cover = Cover.LoadFromDatabase("cover_id", DateTime.Parse("2015-12-12"), DateTime.Parse("2016-12-12"), CoverType.BulkCarrier, 3);

        cover.Id.Should().Be("cover_id");
        cover.StartDate.Should().Be(DateTime.Parse("2015-12-12"));
        cover.EndDate.Should().Be(DateTime.Parse("2016-12-12"));
        cover.Type.Should().Be(CoverType.BulkCarrier);
        cover.Premium.Should().Be(3);
    }
}
