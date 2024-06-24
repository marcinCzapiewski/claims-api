using Claims.Domain.Shared;
using MediatR;

namespace Claims.Application.Claims.Commands;
public sealed record DeleteClaimCommand : IRequest<Result>
{
    public required string ClaimId { get; init; }
    public required string HttpRequestType { get; init; }
}
