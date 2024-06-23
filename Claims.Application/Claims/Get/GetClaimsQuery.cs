using Claims.Domain.Claims;
using MediatR;

namespace Claims.Application.Claims.Get;
public sealed record GetClaimsQuery : IRequest<IReadOnlyCollection<Claim>>;
