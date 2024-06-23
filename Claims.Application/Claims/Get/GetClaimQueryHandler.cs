using Claims.Domain.Claims;
using MediatR;

namespace Claims.Application.Claims.Get;
internal sealed class GetClaimQueryHandler(IClaimsRepository claimsRepository) : IRequestHandler<GetClaimQuery, Claim?>
{
    private readonly IClaimsRepository _claimsRepository = claimsRepository;

    public Task<Claim?> Handle(GetClaimQuery request, CancellationToken cancellationToken)
    {
        return _claimsRepository.GetClaim(request.ClaimId);
    }
}
