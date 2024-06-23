using Claims.Application.Covers;
using Claims.Domain;
using Claims.Domain.Claims;
using Claims.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Claims.Commands;
internal sealed class CreateClaimCommandHandler(ClaimsContext claimsContext, IAuditer auditer) : IRequestHandler<CreateClaimCommand, Result<Claim>>
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

        auditer.AuditClaim(claim.Value.Id, request.HttpRequestType);

        return claim;
    }
}
