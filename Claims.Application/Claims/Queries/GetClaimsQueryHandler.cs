using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Claims.Queries;
internal class GetClaimsQueryHandler(ClaimsContext claimsContext) : IRequestHandler<GetClaimsQuery, IReadOnlyCollection<ClaimDto>>
{
    public async Task<IReadOnlyCollection<ClaimDto>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
    {
        return await claimsContext.Claims
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
