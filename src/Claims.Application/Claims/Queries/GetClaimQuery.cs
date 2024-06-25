using MediatR;

namespace Claims.Application.Claims.Queries;
public sealed record GetClaimQuery(string ClaimId) : IRequest<ClaimDto?>;
