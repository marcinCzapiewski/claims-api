using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ICoversService _claimsContext;
    private readonly ILogger<CoversController> _logger;

    public CoversController(ILogger<CoversController> logger)
    {
        _logger = logger;
    }

    [HttpPost("compute")]
    public async Task<ActionResult> ComputePremiumAsync(DateTime startDate, DateTime endDate, CoverType coverType)
    {
        return Ok(ComputePremium(startDate, endDate, coverType));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
    {
        var results = await _claimsContext.Covers.ToListAsync();
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id)
    {
        var results = await _claimsContext.Covers.ToListAsync();
        return Ok(results.SingleOrDefault(cover => cover.Id == id));
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover cover)
    {
        cover.Id = Guid.NewGuid().ToString();
        cover.Premium = ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
        _claimsContext.Covers.Add(cover);
        await _claimsContext.SaveChangesAsync();
        _auditer.AuditCover(cover.Id, "POST");
        return Ok(cover);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync(string id)
    {
        _auditer.AuditCover(id, "DELETE");
        var cover = await _claimsContext.Covers.Where(cover => cover.Id == id).SingleOrDefaultAsync();
        if (cover is not null)
        {
            _claimsContext.Covers.Remove(cover);
            await _claimsContext.SaveChangesAsync();
        }
    }
}
