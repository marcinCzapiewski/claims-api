using Claims.Domain.Claims;
using MediatR;

namespace Claims.Application.Claims.Queries;
internal sealed class GetClaimQueryHandler(IClaimsRepository claimsRepository) : IRequestHandler<GetClaimQuery, ClaimReadModel?>
{
    public Task<ClaimReadModel?> Handle(GetClaimQuery request, CancellationToken cancellationToken)
    {
        return claimsRepository.GetClaim(request.ClaimId);
    }
}
