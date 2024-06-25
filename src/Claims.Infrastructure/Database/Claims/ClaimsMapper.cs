using Claims.Domain.Claims;
using Claims.Infrastructure.Database.Claims;
using Claims.Infrastructure.Database.Covers;

namespace Claims.Infrastructure.Database.Claims;
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

    public static ClaimReadModel ToReadModel(this ClaimDto claimDto) => new()
    {
        Id = claimDto.Id,
        CoverId = claimDto.CoverId,
        Created = claimDto.Created,
        Name = claimDto.Name,
        Type = (Domain.Claims.ClaimType)claimDto.Type,
        DamageCost = claimDto.DamageCost
    };

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
