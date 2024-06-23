using Claims.Domain;
using Claims.Domain.Covers;
using Claims.Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure;
internal class CoversRepository(ClaimsContext claimsContext) : ICoversRepository
{
    private readonly ClaimsContext _context = claimsContext;

    public async Task CreateCover(Cover cover)
    {
        var coverDto = new CoverDto
        {
            Id = cover.Id.ToString(),
            StartDate = cover.StartDate,
            EndDate = cover.EndDate,
            Premium = cover.Premium,
            Type = (Dtos.CoverType)cover.Type
        };

        _context.Covers.Add(coverDto);

        await _context.SaveChangesAsync();
    }

    public async Task<Cover?> GetCover(string coverId)
    {
        var coverDto = await _context.Covers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == coverId);

        if (coverDto == null)
        {
            return null;
        }

        return Cover.LoadFromDatabase(
            Guid.Parse(coverDto.Id),
            coverDto.StartDate,
            coverDto.EndDate,
            (Domain.Covers.CoverType)coverDto.Type,
            coverDto.Premium);
    }

    public async Task<IReadOnlyCollection<Cover>> GetAllCovers()
    {
        return (await _context.Covers
            .AsNoTracking()
            .Select(x => Cover.LoadFromDatabase(Guid.Parse(x.Id), x.StartDate, x.EndDate, (Domain.Covers.CoverType)x.Type, x.Premium))
            .ToListAsync())
            .AsReadOnly();
    }

    public async Task DeleteCover(string coverId)
    {
        var coverDto = await _context.Covers.Where(cover => cover.Id == coverId).SingleOrDefaultAsync();
        if (coverDto is not null)
        {
            _context.Covers.Remove(coverDto);
            await _context.SaveChangesAsync();
        }
    }
}
