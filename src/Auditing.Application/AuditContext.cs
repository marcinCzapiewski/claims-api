using Auditing.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Auditing.Application
{
    public class AuditContext : DbContext
    {
        public AuditContext(DbContextOptions<AuditContext> options) : base(options)
        {
        }
        public DbSet<ClaimAuditDto> ClaimAudits { get; set; }
        public DbSet<CoverAuditDto> CoverAudits { get; set; }
    }
}
