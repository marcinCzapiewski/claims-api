using Claims.Application.Commands;
using Claims.Domain.Entities;

namespace Claims.Application;
public interface ICoversService
{
    Task<Cover> CreateCover(CreateCoverCommand command);
    Task<IReadOnlyCollection<Cover>> GetAllCovers();
    Task<Cover?> GetCover(string id);
    Task DeleteCover(DeleteCoverCommand command);
}
