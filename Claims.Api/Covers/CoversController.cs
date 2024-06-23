using Claims.Api.Contracts.Requests;
using Claims.Api.Covers;
using Claims.Application.Covers.Create;
using Claims.Application.Covers.Delete;
using Claims.Application.Covers.Get;
using Claims.Domain.Covers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController(ILogger<CoversController> logger, ISender sender) : ControllerBase
{
    private readonly ILogger<CoversController> _logger = logger;
    private readonly ISender _sender = sender;

    [HttpPost("compute")]
    public ActionResult ComputePremium(DateTime startDate, DateTime endDate, Api.Contracts.CoverType coverType)
    {
        var calculatedPremium = Cover.ComputePremium(startDate, endDate, (CoverType)coverType);

        return Ok(calculatedPremium);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Api.Contracts.Cover>>> GetAsync()
    {
        var results = await _sender.Send(new GetCoversQuery());

        var result = results.ToCoverApiContracts();


        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Api.Contracts.Cover>> GetAsync(string id)
    {
        var cover = await _sender.Send(new GetCoverQuery(id));

        if (cover is null)
        {
            return NotFound();
        }

        return Ok(cover.ToCoverApiContract());
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(CreateCoverRequest request)
    {
        var cover = await _sender.Send(new CreateCoverCommand
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = (CoverType)request.Type,
            HttpRequestType = HttpContext.Request.Method
        });

        var locationUri = Url.Link("covers", new { id = cover.Id });
        return Created(locationUri, cover.ToCoverApiContract());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _sender.Send(new DeleteCoverCommand { CoverId = id, HttpRequestType = HttpContext.Request.Method });

        return NoContent();
    }
}
