using Auditing.Application.Dtos;

namespace Auditing.Application;

internal class AuditService(AuditContext auditContext)
{
    private readonly AuditContext _auditContext = auditContext;

    public async Task AuditClaim(string id, string httpRequestType)
    {
        var claimAudit = new ClaimAuditDto()
        {
            Created = DateTime.Now,
            HttpRequestType = httpRequestType,
            ClaimId = id
        };

        _auditContext.Add(claimAudit);
        await _auditContext.SaveChangesAsync();
    }

    public async Task AuditCover(string id, string httpRequestType)
    {
        var coverAudit = new CoverAuditDto()
        {
            Created = DateTime.Now,
            HttpRequestType = httpRequestType,
            CoverId = id
        };

        _auditContext.Add(coverAudit);
        await _auditContext.SaveChangesAsync();
    }
}
