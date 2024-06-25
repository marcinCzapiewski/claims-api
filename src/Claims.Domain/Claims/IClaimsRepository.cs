namespace Claims.Domain.Claims;
public interface IClaimsRepository
{
    public Task AddClaim(Claim claim);
    Task<IReadOnlyCollection<ClaimReadModel>> GetAllClaims();
    public Task<ClaimReadModel?> GetClaim(string claimId);
    public Task DeleteClaim(string claimId);
}
