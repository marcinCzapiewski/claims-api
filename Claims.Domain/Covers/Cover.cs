namespace Claims.Domain.Covers;

public class Cover
{
    public Guid Id { get; private set; }

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public CoverType Type { get; private set; }

    public decimal Premium { get; private set; }

    private Cover(Guid id, DateTime startDate, DateTime endDate, CoverType type, decimal premium)
    {
        Id = id;
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
        Premium = premium;
    }

    public static Cover New(DateTime startDate, DateTime endDate, CoverType type)
    {
        return new Cover(Guid.NewGuid(), startDate, endDate, type, ComputePremium(startDate, endDate, type));
    }

    public static Cover LoadFromDatabase(Guid id, DateTime startDate, DateTime endDate, CoverType type, decimal premium)
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