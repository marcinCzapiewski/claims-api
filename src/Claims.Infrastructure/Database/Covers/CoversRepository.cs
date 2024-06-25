using Claims.Domain.Covers;
using Microsoft.EntityFrameworkCore;

namespace Claims.Infrastructure.Database.Covers;
internal class CoversRepository(ApplicationDbContext claimsContext) : ICoversRepository
{
    private readonly ApplicationDbContext _context = claimsContext;

    public async Task AddCover(Cover cover)
    {
        var coverDto = cover.ToDatabaseModel();

        _context.Covers.Add(coverDto);

        await _context.SaveChangesAsync();
    }

    public async Task<CoverReadModel?> GetCoverReadModel(string coverId)
    {
        var coverDto = await _context.Covers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == coverId);

        if (coverDto == null)
        {
            return null;
        }

        return coverDto.ToReadModel();
    }

    public async Task<Cover?> GetCover(string coverId)
    {
        var coverDto = await _context.Covers
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == coverId);

        if (coverDto == null)
        {
            return null;
        };

        return coverDto.ToDomainModel();
    }

    public async Task<IReadOnlyCollection<CoverReadModel>> GetAllCovers()
    {
        return (await _context.Covers
            .AsNoTracking()
            .ToListAsync())
            .Select(x => x.ToReadModel())
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