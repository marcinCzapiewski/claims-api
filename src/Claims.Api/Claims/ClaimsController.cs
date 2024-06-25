using Claims.Application.Claims;
using Claims.Application.Claims.Commands;
using Claims.Application.Claims.Queries;
using Claims.Domain.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Claims;

[ApiController]
[Route("[controller]")]
public class ClaimsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClaimReadModel>>> GetAsync()
    {
        var results = await _sender.Send(new GetClaimsQuery());

        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClaimReadModel>> GetAsync(string id)
    {
        var claim = await _sender.Send(new GetClaimQuery(id));

        if (claim is null)
        {
            return NotFound();
        }

        return Ok(claim);
    }

    [HttpPost]
    public async Task<ActionResult<ClaimReadModel>> CreateAsync(CreateClaimRequest request)
    {
        var claim = await _sender.Send(new CreateClaimCommand
        {
            CoverId = request.CoverId,
            Name = request.Name,
            ClaimType = request.Type,
            DamageCost = request.DamageCost,
            HttpRequestType = HttpContext.Request.Method
        });

        if (claim.IsFailure)
        {
            return Problem(title: claim.Error.Code, detail: claim.Error.Message, statusCode: StatusCodes.Status400BadRequest);
        }

        var locationUri = Url.Link("claims", new { id = claim.Value.Id });
        return Created(locationUri, claim.Value.ToReadModel());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAsync(string id)
    {
        var result = await _sender.Send(new DeleteClaimCommand { ClaimId = id, HttpRequestType = HttpContext.Request.Method });

        if (result.IsFailure)
        {
            return Problem(title: result.Error.Code, detail: result.Error.Message, statusCode: StatusCodes.Status400BadRequest);
        }

        return NoContent();
    }
}
