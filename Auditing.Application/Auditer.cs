using Auditing.Application.Dtos;
using Claims.Domain;

namespace Auditing.Application;

internal class Auditer(AuditContext auditContext) : IAuditer
{
    private readonly AuditContext _auditContext = auditContext;

    public void AuditClaim(string id, string httpRequestType)
    {
        var claimAudit = new ClaimAuditDto()
        {
            Created = DateTime.Now,
            HttpRequestType = httpRequestType,
            ClaimId = id
        };

        _auditContext.Add(claimAudit);
        _auditContext.SaveChanges();
    }

    public void AuditCover(string id, string httpRequestType)
    {
        var coverAudit = new CoverAuditDto()
        {
            Created = DateTime.Now,
            HttpRequestType = httpRequestType,
            CoverId = id
        };

        _auditContext.Add(coverAudit);
        _auditContext.SaveChanges();
    }
}
