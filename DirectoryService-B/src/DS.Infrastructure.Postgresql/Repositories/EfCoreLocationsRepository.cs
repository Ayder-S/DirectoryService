using CSharpFunctionalExtensions;
using DS.Application.Database;
using DS.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace DS.Infrastructure.Postgresql.Repositories;

public class EfCoreLocationsRepository : ILocationsRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<EfCoreLocationsRepository> _logger;
    
    public EfCoreLocationsRepository(DirectoryServiceDbContext dbContext, ILogger<EfCoreLocationsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid>> Add(Location location, CancellationToken cancellationToken)
    {
        try
        {
            await _dbContext.Locations.AddAsync(location, cancellationToken);
        
            await _dbContext.SaveChangesAsync(cancellationToken);
        
            return location.Id;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            
            return Result.Failure<Guid>(exception.Message);
        }
    }
}