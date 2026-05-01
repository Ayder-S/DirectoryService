using DS.Application.Database;
using DS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
}