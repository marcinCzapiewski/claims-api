using Claims.Domain.Claims;

namespace Claims.Api.Claims;

public record CreateClaimRequest
{
    public required string CoverId { get; init; }
    public required string Name { get; init; }
    public ClaimType Type { get; init; }
    public decimal DamageCost { get; init; }
}
