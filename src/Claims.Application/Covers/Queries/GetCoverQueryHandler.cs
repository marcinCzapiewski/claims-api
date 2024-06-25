using Claims.Domain.Covers;
using MediatR;

namespace Claims.Application.Covers.Queries;
internal sealed class GetCoverQueryHandler(ICoversRepository coversRepository) : IRequestHandler<GetCoverQuery, CoverReadModel?>
{
    public Task<CoverReadModel?> Handle(GetCoverQuery request, CancellationToken cancellationToken)
    {
        return coversRepository.GetCoverReadModel(request.CoverId);
    }
}
