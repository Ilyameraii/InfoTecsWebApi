using Entities;
using Microsoft.EntityFrameworkCore;

namespace Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ValueRecord> Values => Set<ValueRecord>();

    public DbSet<ResultRecord> Results => Set<ResultRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}