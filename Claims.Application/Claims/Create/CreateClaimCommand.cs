using Claims.Domain.Claims;
using MediatR;

namespace Claims.Application.Claims.Create;
public sealed record CreateClaimCommand : IRequest<Claim>
{
    public required string CoverId { get; init; }
    public required string Name { get; init; }
    public ClaimType ClaimType { get; init; }
    public decimal DamageCost { get; init; }
    public required string HttpRequestType { get; init; }
}
