using DS.Application.Database;
using DS.Domain.Entities;
using DS.Domain.Relation;
using Microsoft.EntityFrameworkCore;

namespace DS.Infrastructure.Postgresql;

public class DirectoryServiceDbContext(DbContextOptions<DirectoryServiceDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DirectoryServiceDbContext).Assembly);
    }
    
    public DbSet<Department> Departments => Set<Department>();
    
    public DbSet<Location> Locations => Set<Location>();
    
    public DbSet<Position> Positions => Set<Position>();
    
    public DbSet<DepartmentLocation> DepartmentLocations => Set<DepartmentLocation>();
    
    public DbSet<DepartmentPosition> DepartmentPositions => Set<DepartmentPosition>();
}