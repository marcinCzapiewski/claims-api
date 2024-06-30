using Auditing.Application.Dtos;

namespace Auditing.Application;

internal class AuditService(AuditContext auditContext)
{
    private readonly AuditContext _auditContext = auditContext;

    public async Task AuditClaim(string id, string httpRequestType)
    {
        var claimAudit = new ClaimAuditDto()
        {
            Created = DateTime.UtcNow,
            HttpRequestType = httpRequestType,
            ClaimId = id
        };

        _auditContext.ClaimAudits.Add(claimAudit);
        await _auditContext.SaveChangesAsync();
    }

    public async Task AuditCover(string id, string httpRequestType)
    {
        var coverAudit = new CoverAuditDto()
        {
            Created = DateTime.UtcNow,
            HttpRequestType = httpRequestType,
            CoverId = id
        };

        _auditContext.CoverAudits.Add(coverAudit);
        await _auditContext.SaveChangesAsync();
    }
}
