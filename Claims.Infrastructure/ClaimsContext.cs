using Claims.Infrastructure.Dtos;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.Infrastructure;
internal class ClaimsContext : DbContext
{
    private DbSet<ClaimDto> Claims { get; init; }
    public DbSet<CoverDto> Covers { get; init; }

    public ClaimsContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ClaimDto>().ToCollection("claims");
        modelBuilder.Entity<CoverDto>().ToCollection("covers");
    }
}
