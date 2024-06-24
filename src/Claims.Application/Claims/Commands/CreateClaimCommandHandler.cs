using Claims.Application.Covers;
using Claims.Domain.Claims;
using Claims.Domain.Shared;
using Claims.Events;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Claims.Commands;
internal sealed class CreateClaimCommandHandler(ClaimsContext claimsContext, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateClaimCommand, Result<Claim>>
{
    public async Task<Result<Claim>> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        var relatedCover = await claimsContext.Covers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.CoverId, cancellationToken);

        if (relatedCover == null)
        {
            return Result.Failure<Claim>(new Error(
                "Claim.Creating.CoverId",
                $"Cover with id: {request.CoverId} does not exist"));
        }

        var claim = Claim.New(
            relatedCover.ToDomainModel(),
            request.Name,
            (Domain.Claims.ClaimType)request.ClaimType,
            request.DamageCost);

        if (claim.IsFailure)
        {
            return claim;
        }

        // NOTE: should create and audit be transactional?
        claimsContext.Claims.Add(claim.Value.ToDatabaseModel());
        await claimsContext.SaveChangesAsync(cancellationToken);

        await publishEndpoint.Publish(
            new ClaimCreatedEvent { ClaimId = claim.Value.Id, HttpRequestType = request.HttpRequestType },
            cancellationToken);

        return claim;
    }
}
