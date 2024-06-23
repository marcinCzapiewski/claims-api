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
            return Result.Failure<Cover>(new DomainError("Cover.Creating.StartDate", "Cover's start date cannot be in the past"));
        }

        if (startDate > endDate)
        {
            return Result.Failure<Cover>(new DomainError("Cover.Creating.StartDate", "Cover's start date cannot be later than end date"));
        }

        if ((endDate - startDate).Days > 365)
        {
            return Result.Failure<Cover>(new DomainError("Cover.Creating.InsurancePeriod", "Insurance period cannot exceed 1 year"));
        }

        return new Cover(Guid.NewGuid().ToString(), startDate, endDate, type, ComputePremium(startDate, endDate, type));
    }

    public static Cover LoadFromDatabase(string id, DateTime startDate, DateTime endDate, CoverType type, decimal premium)
    {
        return new Cover(id, startDate, endDate, type, premium);
    }

    public static decimal ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var multiplier = 1.3m;
        if (coverType == CoverType.Yacht)
        {
            multiplier = 1.1m;
        }

        if (coverType == CoverType.PassengerShip)
        {
            multiplier = 1.2m;
        }

        if (coverType == CoverType.Tanker)
        {
            multiplier = 1.5m;
        }

        var premiumPerDay = 1250 * multiplier;
        var insuranceLength = (endDate - startDate).TotalDays;
        var totalPremium = 0m;

        for (var i = 0; i < insuranceLength; i++)
        {
            if (i < 30) totalPremium += premiumPerDay;
            if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
            else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
            if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
            else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
        }

        return totalPremium;
    }
}