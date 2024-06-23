using Claims.Domain;
using MediatR;

namespace Claims.Application.Covers.Delete;
internal sealed class DeleteCoverCommandHandler(ICoversRepository coversRepository, IAuditer auditer) : IRequestHandler<DeleteCoverCommand>
{
    private readonly ICoversRepository _coversRepository = coversRepository;
    private readonly IAuditer _auditer = auditer;

    public async Task Handle(DeleteCoverCommand request, CancellationToken cancellationToken)
    {
        // NOTE: delete and audit possibly should be transactional
        await _coversRepository.DeleteCover(request.CoverId);
        _auditer.AuditCover(request.CoverId, request.HttpRequestType);
    }
}
