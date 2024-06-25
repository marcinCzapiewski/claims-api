using Claims.Application.Covers.Commands;
using Claims.Application.Covers.Queries;
using Claims.Domain.Covers;
using Claims.Application.Covers;
using Claims.Domain.Covers.Premium;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Covers;

[ApiController]
[Route("[controller]")]
public class CoversController(ILogger<CoversController> logger, ISender sender) : ControllerBase
{
    private readonly ILogger<CoversController> _logger = logger;
    private readonly ISender _sender = sender;

    [HttpPost("compute")]
    public ActionResult ComputePremium(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        var calculatedPremium = new CoverPremium(startDate, endDate, coverType);

        return Ok(calculatedPremium.Value);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CoverReadModel>>> GetAsync()
    {
        var results = await _sender.Send(new GetCoversQuery());

        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CoverReadModel>> GetAsync(string id)
    {
        var cover = await _sender.Send(new GetCoverQuery(id));

        if (cover is null)
        {
            return NotFound();
        }

        return Ok(cover);
    }

    [HttpPost]
    public async Task<ActionResult<CoverReadModel>> CreateAsync(CreateCoverRequest request)
    {
        var cover = await _sender.Send(new CreateCoverCommand
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Type = request.Type,
            HttpRequestType = HttpContext.Request.Method
        });

        if (cover.IsFailure)
        {
            return Problem(title: cover.Error.Code, detail: cover.Error.Message, statusCode: StatusCodes.Status400BadRequest);
        }

        var locationUri = Url.Link("covers", new { id = cover.Value.Id });
        return Created(locationUri, cover.Value.ToReadModel());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var result = await _sender.Send(new DeleteCoverCommand { CoverId = id, HttpRequestType = HttpContext.Request.Method });

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message, statusCode: StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }
}
