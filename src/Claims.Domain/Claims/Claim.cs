using Claims.Domain.Covers;
using Claims.Domain.Shared;

namespace Claims.Domain.Claims;
public class Claim
{
    public const decimal MaximumDamageCost = 100_000m;

    public string Id { get; private set; }
    public Cover Cover { get; private set; }
    public DateTime Created { get; private set; }
    public string Name { get; private set; }
    public ClaimType Type { get; private set; }
    public decimal DamageCost { get; private set; }

    private Claim(string id, Cover cover, DateTime created, string name, ClaimType type, decimal damageCost)
    {
        Id = id;
        Cover = cover;
        Created = created;
        Name = name;
        Type = type;
        DamageCost = damageCost;
    }

    public static Result<Claim> New(Cover cover, string name, ClaimType type, decimal damageCost)
    {
        if (damageCost > MaximumDamageCost)
        {
            return Result.Failure<Claim>(DomainErrors.Claims.DamageCostTooHigh);
        }

        var now = DateTime.UtcNow;
        if (NowIsNotWithinRelatedCoverPeriod(cover, now))
        {
            return Result.Failure<Claim>(DomainErrors.Claims.OutsideCoverPeriod);
        }

        return new Claim(Guid.NewGuid().ToString(), cover, now, name, type, damageCost);
    }

    public static Claim LoadFromDatabase(string id, Cover cover, DateTime created, string name, ClaimType type, decimal damageCost)
    {
        return new Claim(id, cover, created, name, type, damageCost);
    }

    private static bool NowIsNotWithinRelatedCoverPeriod(Cover cover, DateTime now) => now < cover.StartDate || now > cover.EndDate;
}
