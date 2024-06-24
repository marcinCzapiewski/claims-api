using Claims.Domain.Shared;

namespace Claims.Domain.Covers;

public class Cover
{
    public string Id { get; private set; }

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public CoverType Type { get; private set; }

    public decimal Premium { get; private set; }

    private Cover(string id, DateTime startDate, DateTime endDate, CoverType type, decimal premium)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
        Premium = premium;
    }

    public static Result<Cover> New(DateTime startDate, DateTime endDate, CoverType type)
    {
        if (startDate < DateTime.UtcNow)
        {
            return Result.Failure<Cover>(DomainErrors.Covers.StartDateInPast);
        }

        if (startDate > endDate)
        {
            return Result.Failure<Cover>(DomainErrors.Covers.StartDateLaterThanEndDate);
        }

        if ((endDate - startDate).Days > 365)
        {
            return Result.Failure<Cover>(DomainErrors.Covers.InsurancePeriodTooLong);
        }

        return new Cover(Guid.NewGuid().ToString(), startDate, endDate, type, ComputePremium(startDate, endDate, type));
    }

    public static Cover LoadFromDatabase(string id, DateTime startDate, DateTime endDate, CoverType type, decimal premium)
    {
        return new Cover(id, startDate, endDate, type, premium);
    }

    public static decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var premiumPerDay = CalculatePremiumPerDay(coverType);

        var insuranceLength = (endDate.Date - startDate.Date).TotalDays + 1;

        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30)
            {
                totalPremium += premiumPerDay;
                continue;
            }

            if (i < 180)
            {
                if (coverType == CoverType.Yacht)
                {
                    totalPremium += premiumPerDay * 0.95m;

                }
                else
                {
                    totalPremium += premiumPerDay * 0.98m;
                }

                continue;
            }

            if (i >= 180)
            {
                if (coverType == CoverType.Yacht)
                {
                    totalPremium += premiumPerDay * 0.95m * 0.97m;
                }
                else
                {
                    totalPremium += premiumPerDay * 0.98m * 0.99m;
                }
            }

        }

        return totalPremium;
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
}