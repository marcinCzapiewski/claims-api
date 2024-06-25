using Claims.Domain.Claims;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Database.Claims;
internal class ClaimsRepository(ApplicationDbContext context) : IClaimsRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddClaim(Claim claim)
    {
        var claimDto = claim.ToDatabaseModel();

        _context.Claims.Add(claimDto);
        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<ClaimReadModel>> GetAllClaims()
    {
        return (await _context.Claims
            .AsNoTracking()
            .Select(x => x.ToReadModel())
            .ToListAsync())
            .AsReadOnly();
    }

    public async Task<ClaimReadModel?> GetClaim(string claimId)
    {
        var claimDto = await _context.Claims
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == claimId);

        if (claimDto == null)
        {
            return null;
        }

        return claimDto.ToReadModel();
    }

    public async Task DeleteClaim(string claimId)
    {
        var claimDto = await _context.Claims.FirstOrDefaultAsync(claim => claim.Id == claimId);
        if (claimDto is not null)
        {
            _context.Claims.Remove(claimDto);
            await _context.SaveChangesAsync();
        }
    }
}
