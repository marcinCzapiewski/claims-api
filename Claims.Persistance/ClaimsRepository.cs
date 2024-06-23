using Claims.Domain.Claims;
using Claims.Persistance;
using Claims.Persistance.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Claims.Persistance;
internal class ClaimsRepository(ClaimsContext context) : IClaimsRepository
{
    private readonly ClaimsContext _context = context;

    public async Task AddClaim(Claim claim)
    {
        var claimDto = new ClaimDto
        {
            Id = claim.Id,
            CoverId = claim.CoverId,
            Created = claim.Created,
            Name = claim.Name,
            Type = (Persistance.Dtos.ClaimType)claim.Type,
            DamageCost = claim.DamageCost
        };

        _context.Claims.Add(claimDto);

        await _context.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<Claim>> GetAllClaims()
    {
        return (await _context.Claims
            .AsNoTracking()
            .ToListAsync())
            .Select(x => Claim.LoadFromDatabase(x.Id, x.CoverId, x.Created, x.Name, (Domain.Claims.ClaimType)x.Type, x.DamageCost))
            .ToList();
    }

    public async Task<Claim?> GetClaim(string claimId)
    {
        var claimDto = await _context.Claims
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == claimId);

        if (claimDto == null)
        {
            return null;
        }

        return Claim.LoadFromDatabase(
            claimDto.Id,
            claimDto.CoverId,
            claimDto.Created,
            claimDto.Name,
            (Domain.Claims.ClaimType)claimDto.Type,
            claimDto.DamageCost);
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
