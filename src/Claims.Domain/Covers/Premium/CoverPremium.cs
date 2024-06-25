namespace Claims.Domain.Covers.Premium;
public class CoverPremium(DateTime startDate, DateTime endDate, CoverType coverType)
{
    public const int FirstTierDiscountDaysThreshold = 30;
    private const int SecondTierDiscountDaysThreshold = 180;

    public decimal Value { get; private set; } = Compute(startDate, endDate, coverType);

    private static decimal Compute(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var premiumPerDay = CalculatePremiumPerDay(coverType);

        var insuranceLength = (endDate.Date - startDate.Date).TotalDays + 1;

        DiscountedPremiumCalculationStrategy strategy = coverType switch
        {
            CoverType.Yacht => new YachtPremiumDiscountCalculationStrategy(),
            _ => new DefaultPremiumDiscountCalculationStrategy()
        };

        return strategy.CalculatePremium(premiumPerDay, (int)insuranceLength);
    }

    private static decimal CalculatePremiumPerDay(CoverType coverType)
    {
        const int baseDayRate = 1250;
        decimal multiplier = GetCoverTypeMultiplier(coverType);

        var premiumPerDay = baseDayRate * multiplier;
        return premiumPerDay;
    }

    private static decimal GetCoverTypeMultiplier(CoverType coverType) => coverType switch
    {
        CoverType.Yacht => 1.1m,
        CoverType.PassengerShip => 1.2m,
        CoverType.Tanker => 1.5m,
        _ => 1.3m
    };

    private abstract class DiscountedPremiumCalculationStrategy
    {
        public abstract decimal CalculatePremium(decimal premiumPerDay, int days);

        protected static decimal CalculatePremium(
            decimal premiumPerDay,
            int days,
            decimal discountRateAfterFirstThreshold,
            decimal discountRateAfterSecondThreshold)
        {
            if (days <= FirstTierDiscountDaysThreshold)
                return days * premiumPerDay;

            if (days <= SecondTierDiscountDaysThreshold)
                return FirstTierDiscountDaysThreshold * premiumPerDay + (days - FirstTierDiscountDaysThreshold) * premiumPerDay * discountRateAfterFirstThreshold;

            return FirstTierDiscountDaysThreshold * premiumPerDay
                 + (SecondTierDiscountDaysThreshold - FirstTierDiscountDaysThreshold) * premiumPerDay * discountRateAfterFirstThreshold
                 + (days - SecondTierDiscountDaysThreshold) * premiumPerDay * discountRateAfterSecondThreshold;
        }
    }

    private class YachtPremiumDiscountCalculationStrategy : DiscountedPremiumCalculationStrategy
    {
        const decimal DiscountRateAfterFirstThreshold = 0.95m;
        const decimal DiscountRateAfterSecondThreshold = 0.95m * 0.97m;

        public override decimal CalculatePremium(decimal premiumPerDay, int days)
        {
            return CalculatePremium(premiumPerDay, days, DiscountRateAfterFirstThreshold, DiscountRateAfterSecondThreshold);
        }
    }

    private class DefaultPremiumDiscountCalculationStrategy : DiscountedPremiumCalculationStrategy
    {
        const decimal DiscountRateAfterFirstThreshold = 0.98m;
        const decimal DiscountRateAfterSecondThreshold = 0.98m * 0.99m;

        public override decimal CalculatePremium(decimal premiumPerDay, int days)
        {
            return CalculatePremium(premiumPerDay, days, DiscountRateAfterFirstThreshold, DiscountRateAfterSecondThreshold);
        }
    }
}
