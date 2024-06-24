using Claims.Application.Claims;
using Claims.Application.Covers;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Claims.Application;
internal class ClaimsContext(DbContextOptions options) : DbContext(options)
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
