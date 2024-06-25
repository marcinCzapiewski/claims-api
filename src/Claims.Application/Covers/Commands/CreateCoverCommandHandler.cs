using Claims.Domain.Covers;
using Claims.Domain.Shared;
using Claims.Events;
using MassTransit;
using MediatR;

namespace Claims.Application.Covers.Commands;
internal sealed class CreateCoverCommandHandler(ICoversRepository coversRepository, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateCoverCommand, Result<Cover>>
{
    public async Task<Result<Cover>> Handle(CreateCoverCommand request, CancellationToken cancellationToken)
    {
        var coverResult = Cover.New(request.StartDate, request.EndDate, request.Type);

        if (coverResult.IsFailure)
        {
            return coverResult;
        }

        // NOTE: should create and audit be transactional?
        await coversRepository.AddCover(coverResult.Value);

        await publishEndpoint.Publish(
            new CoverCreatedEvent { CoverId = coverResult.Value.Id, HttpRequestType = request.HttpRequestType },
            cancellationToken);

        return coverResult;
    }
}
