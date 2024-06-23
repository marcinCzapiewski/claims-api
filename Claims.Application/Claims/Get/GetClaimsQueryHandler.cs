using Claims.Domain.Claims;
using MediatR;

namespace Claims.Application.Claims.Get;
internal class GetClaimsQueryHandler(IClaimsRepository claimsRepository) : IRequestHandler<GetClaimsQuery, IReadOnlyCollection<Claim>>
{
    private readonly IClaimsRepository _claimsRepository = claimsRepository;

    public Task<IReadOnlyCollection<Claim>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
    {
        return _claimsRepository.GetAllClaims();
    }
}
