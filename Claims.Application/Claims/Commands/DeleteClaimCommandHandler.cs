using Claims.Domain;
using Claims.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Claims.Application.Claims.Commands;
internal sealed class DeleteClaimCommandHandler(ClaimsContext claimsContext, IAuditer auditer) : IRequestHandler<DeleteClaimCommand, Result>
{
    public async Task<Result> Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        // NOTE: delete and audit possibly should be transactional
        var claimDto = await claimsContext.Claims
            .FirstOrDefaultAsync(claim => claim.Id == request.ClaimId, cancellationToken);

        if (claimDto is not null)
        {
            claimsContext.Claims.Remove(claimDto);
            await claimsContext.SaveChangesAsync(cancellationToken);
        }

        auditer.AuditClaim(request.ClaimId, request.HttpRequestType);

        return Result.Success();
    }
}
