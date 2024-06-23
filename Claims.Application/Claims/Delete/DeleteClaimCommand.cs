using MediatR;

namespace Claims.Application.Claims.Delete;
public sealed record DeleteClaimCommand : IRequest
{
    public required string ClaimId { get; init; }
    public required string HttpRequestType { get; init; }
}
