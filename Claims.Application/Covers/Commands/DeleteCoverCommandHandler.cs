using Claims.Domain;
using Claims.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Covers.Commands;
internal sealed class DeleteCoverCommandHandler(ClaimsContext claimsContext, IAuditer auditer) : IRequestHandler<DeleteCoverCommand, Result>
{
    public async Task<Result> Handle(DeleteCoverCommand request, CancellationToken cancellationToken)
    {
        // NOTE: delete and audit possibly should be transactional
        var coverDto = await claimsContext.Covers.FirstOrDefaultAsync(cover => cover.Id == request.CoverId, cancellationToken);
        if (coverDto is not null)
        {
            claimsContext.Covers.Remove(coverDto);
            await claimsContext.SaveChangesAsync(cancellationToken);
        }

        auditer.AuditCover(request.CoverId, request.HttpRequestType);

        return Result.Success();
    }
}
