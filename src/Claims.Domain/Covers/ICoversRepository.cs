namespace Claims.Domain.Covers;
public interface ICoversRepository
{
    public Task AddCover(Cover cover);
    public Task<IReadOnlyCollection<CoverReadModel>> GetAllCovers();
    public Task<CoverReadModel?> GetCoverReadModel(string coverId);
    public Task<Cover?> GetCover(string coverId);
    public Task DeleteCover(string coverId);
}
