using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Covers.Queries;
internal class GetCoversQueryHandler(ClaimsContext claimsContext) : IRequestHandler<GetCoversQuery, IReadOnlyCollection<CoverDto>>
{
    public async Task<IReadOnlyCollection<CoverDto>> Handle(GetCoversQuery request, CancellationToken cancellationToken)
    {
        return await claimsContext.Covers
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);
    }
}
