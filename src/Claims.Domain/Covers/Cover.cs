using Claims.Domain.Covers.Premium;
using Claims.Domain.Shared;

namespace Claims.Domain.Covers;

public class Cover
{
    public string Id { get; private set; }

    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public CoverType Type { get; private set; }

    public CoverPremium Premium { get; private set; }

    private Cover(string id, DateTime startDate, DateTime endDate, CoverType type, CoverPremium premium)
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

        return new Cover(Guid.NewGuid().ToString(), startDate, endDate, type, new CoverPremium(startDate, endDate, type));
    }

    public static Cover LoadFromDatabase(string id, DateTime startDate, DateTime endDate, CoverType type)
    {
        return new Cover(id, startDate, endDate, type, new CoverPremium(startDate, endDate, type));
    }
}