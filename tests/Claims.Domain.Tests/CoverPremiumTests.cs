using Claims.Domain.Covers;
using Claims.Domain.Covers.Premium;
using FluentAssertions;

namespace Claims.Domain.Tests;
public class CoverPremiumTests
{
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
    public void CoverPremium_ForCoverShorterOrEqualTo30Days_Should_BaseOnPeriodLengthAndType(CoverType coverType, int coverDays, decimal expectedPremium)
    {
        var tomorrow = DateTime.UtcNow.AddDays(1);
        var startDate = tomorrow;
        var endDate = tomorrow.AddDays(coverDays - 1);

        var premium = new CoverPremium(startDate, endDate, coverType);

        premium.Value.Should().Be(expectedPremium);
    }

    [Theory]
    [InlineData(CoverType.Yacht, 31, 30 * 1375 + 1 * (1375 * 0.95))]
    [InlineData(CoverType.BulkCarrier, 31, 30 * 1625 + 1 * (1625 * 0.98))]
    [InlineData(CoverType.ContainerShip, 31, 30 * 1625 + 1 * (1625 * 0.98))]
    [InlineData(CoverType.PassengerShip, 31, 30 * 1500 + 1 * (1500 * 0.98))]
    [InlineData(CoverType.Tanker, 31, 30 * 1875 + 1 * (1875 * 0.98))]
    [InlineData(CoverType.Yacht, 180, 30 * 1375 + 150 * (1375 * 0.95))]
    [InlineData(CoverType.BulkCarrier, 180, 30 * 1625 + 150 * (1625 * 0.98))]
    [InlineData(CoverType.ContainerShip, 180, 30 * 1625 + 150 * (1625 * 0.98))]
    [InlineData(CoverType.PassengerShip, 180, 30 * 1500 + 150 * (1500 * 0.98))]
    [InlineData(CoverType.Tanker, 180, 30 * 1875 + 150 * (1875 * 0.98))]
    public void CoverPremium_ForCoverLongerThan30DaysAndShorterOrEqual180ThanDays_Should_BeCalculatedBasedOnPeriodLengthAndType_AndApplyDiscountAfter30Days(CoverType coverType, int coverDays, decimal expectedPremium)
    {
        var tomorrow = DateTime.UtcNow.AddDays(1);
        var startDate = tomorrow;
        var endDate = tomorrow.AddDays(coverDays - 1);

        var premium = new CoverPremium(startDate, endDate, coverType);

        premium.Value.Should().Be(expectedPremium);
    }

    [Theory]
    [InlineData(CoverType.Yacht, 181, 30 * 1375 + 150 * (1375 * 0.95) + 1 * (1375 * 0.95 * 0.97))]
    [InlineData(CoverType.BulkCarrier, 181, 30 * 1625 + 150 * (1625 * 0.98) + 1 * (1625 * 0.98 * 0.99))]
    [InlineData(CoverType.ContainerShip, 181, 30 * 1625 + 150 * (1625 * 0.98) + 1 * (1625 * 0.98 * 0.99))]
    [InlineData(CoverType.PassengerShip, 181, 30 * 1500 + 150 * (1500 * 0.98) + 1 * (1500 * 0.98 * 0.99))]
    [InlineData(CoverType.Tanker, 181, 30 * 1875 + 150 * (1875 * 0.98) + 1 * (1875 * 0.98 * 0.99))]
    public void CoverPremium_ForCoverLongerThan180Days_Should_BeCalculatedBasedOnPeriodLengthAndType_AndApplyDiscountProgressively(CoverType coverType, int coverDays, decimal expectedPremium)
    {
        var tomorrow = DateTime.UtcNow.AddDays(1);
        var startDate = tomorrow;
        var endDate = tomorrow.AddDays(coverDays - 1);

        var premium = new CoverPremium(startDate, endDate, coverType);

        premium.Value.Should().Be(expectedPremium);
    }
}
