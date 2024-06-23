using Claims.Api.Contracts;

namespace Claims.Api.Claims.Contracts.Requests;

public record CreateClaimRequest
{
    public string CoverId { get; init; }
    public string Name { get; init; }
    public ClaimType Type { get; init; }
    public decimal DamageCost { get; init; }
}
