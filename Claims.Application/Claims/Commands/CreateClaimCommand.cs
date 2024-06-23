using Claims.Domain.Claims;
using Claims.Domain.Shared;
using MediatR;

namespace Claims.Application.Claims.Commands;
public sealed record CreateClaimCommand : IRequest<Result<Claim>>
{
    public required string CoverId { get; init; }
    public required string Name { get; init; }
    public ClaimType ClaimType { get; init; }
    public decimal DamageCost { get; init; }
    public required string HttpRequestType { get; init; }
}
