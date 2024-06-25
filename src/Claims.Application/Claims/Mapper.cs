using Claims.Domain.Claims;

namespace Claims.Application.Claims;
public static class Mapper
{
    public static ClaimReadModel ToReadModel(this Claim claim) => new()
    {
        Id = claim.Id,
        CoverId = claim.Cover.Id,
        Created = claim.Created,
        DamageCost = claim.DamageCost,
        Name = claim.Name,
        Type = claim.Type
    };
}
