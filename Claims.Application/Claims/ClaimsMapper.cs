using Claims.Application.Claims;
using Claims.Application.Covers;
using Claims.Domain.Claims;

namespace Claims.Application.Claims;
internal static class ClaimsMapper
{
    public static Claim ToDomainModel(this ClaimDto claimDto, CoverDto coverDto)
    {
        return Claim.LoadFromDatabase(
            claimDto.Id,
            coverDto.ToDomainModel(),
            claimDto.Created,
            claimDto.Name,
            (Domain.Claims.ClaimType)claimDto.Type,
            claimDto.DamageCost);
    }

    public static ClaimDto ToDatabaseModel(this Claim claim) => new()
    {
        Id = claim.Id,
        CoverId = claim.Cover.Id,
        Created = claim.Created,
        Name = claim.Name,
        Type = (ClaimType)claim.Type,
        DamageCost = claim.DamageCost
    };
}
