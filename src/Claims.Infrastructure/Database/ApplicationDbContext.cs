using Claims.Infrastructure.Database.Claims;
using Claims.Infrastructure.Database.Covers;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.Infrastructure.Database;
internal class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ClaimDto> Claims { get; init; }
    public DbSet<CoverDto> Covers { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ClaimDto>().ToCollection("claims");
        modelBuilder.Entity<CoverDto>().ToCollection("covers");
    }
}
