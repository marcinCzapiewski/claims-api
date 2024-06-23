using Claims.Application.Claims;

namespace Claims.Api.Claims;

internal static class Mapper
{
    public static Api.Contracts.Claim ToClaimApiContract(this Domain.Claims.Claim claim) => new()
    {
        Id = claim.Id,
        CoverId = claim.Cover.Id,
        Created = claim.Created,
        DamageCost = claim.DamageCost,
        Name = claim.Name,
        Type = (Api.Contracts.ClaimType)claim.Type
    };

    public static Api.Contracts.Claim ToClaimApiContract(this ClaimDto claim) => new()
    {
        Id = claim.Id,
        CoverId = claim.CoverId,
        Created = claim.Created,
        DamageCost = claim.DamageCost,
        Name = claim.Name,
        Type = (Api.Contracts.ClaimType)claim.Type
    };

    public static IEnumerable<Api.Contracts.Claim> ToClaimApiContracts(this IEnumerable<ClaimDto> claims) =>
        claims.Select(ToClaimApiContract);
}
