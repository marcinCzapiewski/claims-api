using Claims.Domain;
using Claims.Domain.Claims;
using MediatR;

namespace Claims.Application.Claims.Delete;
internal sealed class DeleteClaimCommandHandler : IRequestHandler<DeleteClaimCommand>
{
    private readonly IClaimsRepository _claimsRepository;
    private readonly IAuditer _auditer;

    public DeleteClaimCommandHandler(IClaimsRepository claimsRepository, IAuditer auditer)
    {
        _claimsRepository = claimsRepository;
        _auditer = auditer;
    }

    public async Task Handle(DeleteClaimCommand request, CancellationToken cancellationToken)
    {
        // NOTE: delete and audit possibly should be transactional
        await _claimsRepository.DeleteClaim(request.ClaimId);
        _auditer.AuditClaim(request.ClaimId, request.HttpRequestType);
    }
}
