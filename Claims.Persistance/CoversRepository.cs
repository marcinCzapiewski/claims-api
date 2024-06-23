using Claims.Domain;
using Claims.Domain.Covers;
using Claims.Persistance.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Claims.Persistance;
internal class CoversRepository(ClaimsContext claimsContext) : ICoversRepository
{
    private readonly ClaimsContext _context = claimsContext;

    public async Task AddCover(Cover cover)
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
            coverDto.Id,
            coverDto.StartDate,
            coverDto.EndDate,
            (Domain.Covers.CoverType)coverDto.Type,
            coverDto.Premium);
    }

    public async Task<IReadOnlyCollection<Cover>> GetAllCovers()
    {
        return (await _context.Covers
            .AsNoTracking()
            .ToListAsync())
            .Select(x => Cover.LoadFromDatabase(x.Id, x.StartDate, x.EndDate, (Domain.Covers.CoverType)x.Type, x.Premium))
            .ToList()
            .AsReadOnly();
    }

    public async Task DeleteCover(string coverId)
    {
        var coverDto = await _context.Covers.FirstOrDefaultAsync(cover => cover.Id == coverId);
        if (coverDto is not null)
        {
            _context.Covers.Remove(coverDto);
            await _context.SaveChangesAsync();
        }
    }
}
