using Claims.Domain.Shared;
using Claims.Events;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Covers.Commands;
internal sealed class DeleteCoverCommandHandler(ClaimsContext claimsContext, IPublishEndpoint publishEndpoint) : IRequestHandler<DeleteCoverCommand, Result>
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
        await publishEndpoint.Publish(
            new CoverDeletedEvent { CoverId = request.CoverId, HttpRequestType = request.HttpRequestType },
            cancellationToken);

        return Result.Success();
    }
}
