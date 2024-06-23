namespace Claims.Domain.Claims;
public class Claim
{
    public string Id { get; private set; }
    public string CoverId { get; private set; }
    public DateTime Created { get; private set; }
    public string Name { get; private set; }
    public ClaimType Type { get; private set; }
    public decimal DamageCost { get; private set; }

    private Claim(string id, string coverId, DateTime created, string name, ClaimType type, decimal damageCost)
    {
        Id = id;
        CoverId = coverId;
        Created = created;
        Name = name;
        Type = type;
        DamageCost = damageCost;
    }

    public static Claim New(string coverId, string name, ClaimType type, decimal damageCost)
    {
        return new Claim(Guid.NewGuid().ToString(), coverId, DateTime.UtcNow, name, type, damageCost);
    }

    public static Claim LoadFromDatabase(string id, string coverId, DateTime created, string name, ClaimType type, decimal damageCost)
    {
        return new Claim(id, coverId, created, name, type, damageCost);
    }
}
