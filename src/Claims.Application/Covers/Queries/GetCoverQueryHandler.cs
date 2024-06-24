using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Covers.Queries;
internal sealed class GetCoverQueryHandler(ClaimsContext claimsContext) : IRequestHandler<GetCoverQuery, CoverDto?>
{
    public Task<CoverDto?> Handle(GetCoverQuery request, CancellationToken cancellationToken)
    {
        return claimsContext.Covers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CoverId, cancellationToken);
    }
}
