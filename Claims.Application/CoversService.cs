using Claims.Application.Commands;
using Claims.Domain;
using Claims.Domain.Entities;

namespace Claims.Application;
internal class CoversService(ICoversRepository coversRepository, IAuditer auditer) : ICoversService
{
    private readonly ICoversRepository _coversRepository = coversRepository;
    private readonly IAuditer _audider = auditer;

    public async Task<Cover> CreateCover(CreateCoverCommand command)
    {
        // TODO validation and error handling
        var cover = Cover.New(command.StartDate, command.EndDate, command.Type);

        // NOTE: create and audit should be transactional
        await _coversRepository.CreateCover(cover);

        _audider.AuditCover(cover.Id.ToString(), command.HttpRequestType);

        return cover;
    }

    public Task<IReadOnlyCollection<Cover>> GetAllCovers()
    {
        return _coversRepository.GetAllCovers();
    }

    public Task<Cover?> GetCover(string id)
    {
        return _coversRepository.GetCover(id);
    }

    public async Task DeleteCover(DeleteCoverCommand command)
    {
        // NOTE: delete and audit should be transactional
        await _coversRepository.DeleteCover(command.CoverId);
        _audider.AuditCover(command.CoverId, command.HttpRequestType);
    }
}
