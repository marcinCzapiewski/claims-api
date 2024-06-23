using Claims.Domain;
using Claims.Domain.Covers;
using MediatR;

namespace Claims.Application.Covers.Create;
internal sealed class CreateCoverCommandHandler(ICoversRepository coversRepository, IAuditer auditer) : IRequestHandler<CreateCoverCommand, Cover>
{
    private readonly ICoversRepository _coversRepository = coversRepository;
    private readonly IAuditer _auditer = auditer;

    public async Task<Cover> Handle(CreateCoverCommand request, CancellationToken cancellationToken)
    {
        // TODO validation and error handling
        var cover = Cover.New(request.StartDate, request.EndDate, request.Type);

        // NOTE: create and audit should be transactional
        await _coversRepository.CreateCover(cover);
        _auditer.AuditCover(cover.Id.ToString(), request.HttpRequestType);

        return cover;
    }
}
