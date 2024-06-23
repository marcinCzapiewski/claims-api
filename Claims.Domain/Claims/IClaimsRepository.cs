namespace Claims.Domain.Claims;
public interface IClaimsRepository
{
    public Task AddClaim(Claim claim);
    Task<IReadOnlyCollection<Claim>> GetAllClaims();
    public Task<Claim?> GetClaim(string claimId);
    public Task DeleteClaim(string claimId);
}
