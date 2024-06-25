using Claims.Domain.Covers;
using Claims.Domain.Shared;
using Claims.Events;
using MassTransit;
using MediatR;

namespace Claims.Application.Covers.Commands;
internal sealed class DeleteCoverCommandHandler(ICoversRepository coversRepository, IPublishEndpoint publishEndpoint) : IRequestHandler<DeleteCoverCommand, Result>
{
    public async Task<Result> Handle(DeleteCoverCommand request, CancellationToken cancellationToken)
    {
        // NOTE: delete and audit possibly should be transactional
        await coversRepository.DeleteCover(request.CoverId);

        await publishEndpoint.Publish(
            new CoverDeletedEvent { CoverId = request.CoverId, HttpRequestType = request.HttpRequestType },
            cancellationToken);

        return Result.Success();
    }
}
