using MediatR;

namespace Claims.Application.Claims.Queries;
public sealed record GetClaimsQuery : IRequest<IReadOnlyCollection<ClaimDto>>;
