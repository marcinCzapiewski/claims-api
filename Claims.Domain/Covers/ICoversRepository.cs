using Claims.Domain.Covers;

namespace Claims.Domain;
public interface ICoversRepository
{
    public Task AddCover(Cover cover);
    public Task<IReadOnlyCollection<Cover>> GetAllCovers();
    public Task<Cover?> GetCover(string coverId);
    public Task DeleteCover(string coverId);
}
