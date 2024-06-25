using Claims.Domain.Claims;
using Claims.Domain.Covers;
using Claims.Domain.Shared;
using Claims.Events;
using MassTransit;
using MediatR;

namespace Claims.Application.Claims.Commands;
internal sealed class CreateClaimCommandHandler(
    IClaimsRepository claimsRepository,
    ICoversRepository coversRepository,
    IPublishEndpoint publishEndpoint)
        : IRequestHandler<CreateClaimCommand, Result<Claim>>
{
    public async Task<Result<Claim>> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        var relatedCover = await coversRepository.GetCover(request.CoverId);

        if (relatedCover == null)
        {
            return Result.Failure<Claim>(new Error(
                "Claim.Creating.CoverId",
                $"Cover with id: {request.CoverId} does not exist"));
        }

        var claim = Claim.New(
            relatedCover,
            request.Name,
            request.ClaimType,
            request.DamageCost);

        if (claim.IsFailure)
        {
            return claim;
        }

        // NOTE: should create and audit be transactional?
        await claimsRepository.AddClaim(claim.Value);

        await publishEndpoint.Publish(
            new ClaimCreatedEvent { ClaimId = claim.Value.Id, HttpRequestType = request.HttpRequestType },
            cancellationToken);

        return claim;
    }
}
