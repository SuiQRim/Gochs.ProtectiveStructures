using Gochs.ProtectiveStructures.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gochs.ProtectiveStructures.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ProtectiveStructure> ProtectiveStructures => Set<ProtectiveStructure>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Inspection> Inspections => Set<Inspection>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
