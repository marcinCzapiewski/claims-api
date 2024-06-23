using Claims.Domain;
using Claims.Domain.Covers;
using Claims.Domain.Shared;
using MediatR;

namespace Claims.Application.Covers.Commands;
internal sealed class CreateCoverCommandHandler(ClaimsContext claimsContext, IAuditer auditer) : IRequestHandler<CreateCoverCommand, Result<Cover>>
{
    public async Task<Result<Cover>> Handle(CreateCoverCommand request, CancellationToken cancellationToken)
    {
        var cover = Cover.New(request.StartDate, request.EndDate, (Domain.Covers.CoverType)request.Type);

        if (cover.IsFailure)
        {
            return cover;
        }

        // NOTE: should create and audit be transactional?
        claimsContext.Covers.Add(cover.Value.ToDatabaseModel());
        await claimsContext.SaveChangesAsync(cancellationToken);
        auditer.AuditCover(cover.Value.Id.ToString(), request.HttpRequestType);

        return cover;
    }
}
