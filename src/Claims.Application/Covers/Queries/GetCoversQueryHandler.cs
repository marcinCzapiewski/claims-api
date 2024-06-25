using Claims.Domain.Covers;
using MediatR;

namespace Claims.Application.Covers.Queries;
internal class GetCoversQueryHandler(ICoversRepository coversRepository) : IRequestHandler<GetCoversQuery, IReadOnlyCollection<CoverReadModel>>
{
    public Task<IReadOnlyCollection<CoverReadModel>> Handle(GetCoversQuery request, CancellationToken cancellationToken)
    {
        return coversRepository.GetAllCovers();
    }
}
