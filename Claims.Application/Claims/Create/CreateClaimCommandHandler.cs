using Claims.Domain;
using Claims.Domain.Claims;
using MediatR;

namespace Claims.Application.Claims.Create;
internal sealed class CreateClaimCommandHandler(IClaimsRepository claimsRepository, IAuditer auditer) : IRequestHandler<CreateClaimCommand, Claim>
{
    private readonly IClaimsRepository _claimsRepository = claimsRepository;
    private readonly IAuditer _auditer = auditer;

    public async Task<Claim> Handle(CreateClaimCommand request, CancellationToken cancellationToken)
    {
        // TODO validation and error handling
        var claim = Claim.New(request.CoverId, request.Name, request.ClaimType, request.DamageCost);

        // NOTE: should create and audit be transactional?
        await _claimsRepository.AddClaim(claim);
        _auditer.AuditClaim(claim.Id, request.HttpRequestType);

        return claim;
    }
}
