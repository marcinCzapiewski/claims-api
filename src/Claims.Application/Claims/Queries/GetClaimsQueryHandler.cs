using Claims.Domain.Claims;
using MediatR;

namespace Claims.Application.Claims.Queries;
internal class GetClaimsQueryHandler(IClaimsRepository claimsRepository) : IRequestHandler<GetClaimsQuery, IReadOnlyCollection<ClaimReadModel>>
{
    public Task<IReadOnlyCollection<ClaimReadModel>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
    {
        return claimsRepository.GetAllClaims();
    }
}
