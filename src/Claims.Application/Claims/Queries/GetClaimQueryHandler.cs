using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Claims.Queries;
internal sealed class GetClaimQueryHandler(ClaimsContext claimsContext) : IRequestHandler<GetClaimQuery, ClaimDto?>
{
    public Task<ClaimDto?> Handle(GetClaimQuery request, CancellationToken cancellationToken)
    {
        return claimsContext.Claims
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.ClaimId, cancellationToken);
    }
}
