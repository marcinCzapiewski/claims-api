using Claims.Domain.Claims;
using Claims.Domain.Shared;
using Claims.Events;
using MassTransit;
using MediatR;

namespace Claims.Application.Claims.Commands;
internal sealed class DeleteClaimCommandHandler(IClaimsRepository claimsRepository, IPublishEndpoint publishEndpoint) : IRequestHandler<DeleteClaimCommand, Result>
{
    public async Task<Result> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        // NOTE: delete and audit possibly should be transactional
        await claimsRepository.DeleteClaim(request.ClaimId);

        await publishEndpoint.Publish(new ClaimDeletedEvent { ClaimId = request.ClaimId, HttpRequestType = request.HttpRequestType }, cancellationToken);

        return Result.Success();
    }
}
