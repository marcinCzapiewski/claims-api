namespace Claims.Api.Claims;

internal static class Mapper
{
    public static Api.Contracts.Claim ToClaimApiContract(this Domain.Claims.Claim claim) => new()
    {
        Id = claim.Id,
        CoverId = claim.CoverId,
        Created = claim.Created,
        DamageCost = claim.DamageCost,
        Name = claim.Name,
        Type = (Api.Contracts.ClaimType)claim.Type
    };

    public static IEnumerable<Api.Contracts.Claim> ToClaimApiContracts(this IEnumerable<Domain.Claims.Claim> claims) =>
        claims.Select(ToClaimApiContract);
}
