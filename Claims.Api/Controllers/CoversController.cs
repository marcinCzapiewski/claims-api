using Claims.Api.Contracts.Requests;
using Claims.Application;
using Claims.Application.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController(ILogger<CoversController> logger, ICoversService coversService) : ControllerBase
{
    private readonly ICoversService _coversService = coversService;
    private readonly ILogger<CoversController> _logger = logger;

    [HttpPost("compute")]
    public ActionResult ComputePremium(DateTime startDate, DateTime endDate, Api.Contracts.CoverType coverType)
    {
        var calculatedPremium = Domain.Entities.Cover.ComputePremium(startDate, endDate, (Domain.Entities.CoverType)coverType);

        return Ok(calculatedPremium);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Api.Contracts.Cover>>> GetAsync()
    {
        var results = (await _coversService
            .GetAllCovers())
            .Select(x => new Api.Contracts.Cover
            {
                Id = x.Id,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Premium = x.Premium,
                Type = (Api.Contracts.CoverType)x.Type
            });

        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Api.Contracts.Cover>> GetAsync(string id)
    {
        var cover = await _coversService.GetCover(id);

        if (cover == null)
        {
            return NotFound();
        }

        return Ok(new Api.Contracts.Cover
        {
            Id = cover.Id,
            StartDate = cover.StartDate,
            EndDate = cover.EndDate,
            Premium = cover.Premium,
            Type = (Api.Contracts.CoverType)cover.Type
        });
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateCoverRequest request)
    {
        var cover = await _coversService.CreateCover(new CreateCoverCommand
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = (Domain.Entities.CoverType)request.Type,
            HttpRequestType = HttpContext.Request.Method
        });

        return Ok(cover);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(string id)
    {
        await _coversService.DeleteCover(new DeleteCoverCommand { CoverId = id, HttpRequestType = HttpContext.Request.Method });
    }
}
