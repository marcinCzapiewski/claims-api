using Claims.Domain;
using Claims.Domain.Covers;
using MediatR;

namespace Claims.Application.Covers.Get;
internal class GetCoversQueryHandler(ICoversRepository coversRepository) : IRequestHandler<GetCoversQuery, IReadOnlyCollection<Cover>>
{
    private readonly ICoversRepository _coversRepository = coversRepository;

    public Task<IReadOnlyCollection<Cover>> Handle(GetCoversQuery request, CancellationToken cancellationToken)
    {
        return _coversRepository.GetAllCovers();
    }
}
