using Claims.Api.Claims.Contracts.Requests;
using Claims.Application.Claims.Commands;
using Claims.Application.Claims.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Claims.Api.Claims;

[ApiController]
[Route("[controller]")]
public class ClaimsController(ILogger<ClaimsController> logger, ISender sender) : ControllerBase
{
    private readonly ILogger<ClaimsController> _logger = logger;
    private readonly ISender _sender = sender;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Api.Contracts.Claim>>> GetAsync()
    {
        var results = await _sender.Send(new GetClaimsQuery());

        return Ok(results.ToClaimApiContracts());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Api.Contracts.Claim>> GetAsync(string id)
    {
        var claim = await _sender.Send(new GetClaimQuery(id));

        if (claim is null)
        {
            return NotFound();
        }

        return Ok(claim.ToClaimApiContract());
    }

    [HttpPost]
    public async Task<ActionResult<Api.Contracts.Claim>> CreateAsync(CreateClaimRequest request)
    {
        var claim = await _sender.Send(new CreateClaimCommand
        {
            CoverId = request.CoverId,
            Name = request.Name,
            ClaimType = (Application.Claims.ClaimType)request.Type,
            DamageCost = request.DamageCost,
            HttpRequestType = HttpContext.Request.Method
        });

        if (claim.IsFailure)
        {
            return Problem(title: claim.Error.Code, detail: claim.Error.Message, statusCode: StatusCodes.Status400BadRequest);
        }

        var locationUri = Url.Link("claims", new { id = claim.Value.Id });
        return Created(locationUri, claim.Value.ToClaimApiContract());
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
