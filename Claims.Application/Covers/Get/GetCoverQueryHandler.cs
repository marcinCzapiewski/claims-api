using Claims.Domain;
using Claims.Domain.Covers;
using MediatR;

namespace Claims.Application.Covers.Get;
internal sealed class GetCoverQueryHandler(ICoversRepository coversRepository) : IRequestHandler<GetCoverQuery, Cover?>
{
    private readonly ICoversRepository _coversRepository = coversRepository;

    public Task<Cover?> Handle(GetCoverQuery request, CancellationToken cancellationToken)
    {
        return _coversRepository.GetCover(request.CoverId);
    }
}
