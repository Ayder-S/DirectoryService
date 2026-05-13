using CSharpFunctionalExtensions;
using DS.Application.Database;
using DS.Domain.Entities;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

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

    public async Task<Result<Guid, Error>> Add(Location location, CancellationToken cancellationToken)
    {
        try
        {
            _dbContext.Locations.Add(location);
        
            await _dbContext.SaveChangesAsync(cancellationToken);
        
            return location.Id;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось сохранить Локацию {LocationId}",  location.Id);
            
            return Error.Failure("location.add.failed", "Не удалось сохранить локацию");
        }
    }
}