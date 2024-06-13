using Claims.Domain.Entities;

namespace Claims.Domain;
public interface ICoversRepository
{
    public Task CreateCover(Cover cover);
    public Task<IReadOnlyCollection<Cover>> GetAllCovers();
    public Task<Cover?> GetCover(string coverId);
    public Task DeleteCover(string coverId);
}
