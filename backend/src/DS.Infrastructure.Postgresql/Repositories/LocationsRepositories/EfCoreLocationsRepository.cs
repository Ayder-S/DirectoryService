using CSharpFunctionalExtensions;
using DS.Application.Database;
using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

namespace DS.Infrastructure.Postgresql.Repositories.LocationsRepositories;

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
    
    public async Task<Result<Location, Error>> GetById(Guid locationId, CancellationToken cancellationToken)
    {
        try
        {
            var location = await _dbContext.Locations.FirstOrDefaultAsync(l => l.Id == locationId, cancellationToken);

            if (location is null)
                return Error.NotFound("location.not_found", $"Локация {locationId} не найдена");

            return location;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось получить локацию {LocationId}", locationId);
            return Error.Failure("location.get.failed", "Не удалось получить локацию");
        }
    }

    public async Task<UnitResult<Error>> Update(
        Guid locationId,
        Name name,
        Address address,
        Timezone timezone,
        CancellationToken cancellationToken)
    {
        try
        {
            int rowsAffected = await _dbContext.Locations
                .Where(l => l.Id == locationId)
                .ExecuteUpdateAsync(
                    setter => setter
                        .SetProperty(l => l.Name, name)
                        .SetProperty(l => l.Address, address)
                        .SetProperty(l => l.Timezone, timezone)
                        .SetProperty(l => l.UpdatedAt, DateTime.UtcNow),
                    cancellationToken);

            if (rowsAffected == 0)
                return Error.NotFound("location.not_found", $"Локация {locationId} не найдена");

            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось обновить локацию {LocationId}", locationId);
            return Error.Failure("location.update.failed", "Не удалось обновить локацию");
        }
    }

    public async Task<UnitResult<Error>> UpdateName(Guid locationId, Name name, CancellationToken cancellationToken)
    {
        try
        {
            int rowsAffected = await _dbContext.Locations
                .Where(l => l.Id == locationId)
                .ExecuteUpdateAsync(
                    setter => setter
                        .SetProperty(l => l.Name, name)
                        .SetProperty(l => l.UpdatedAt, DateTime.UtcNow), cancellationToken);

            if (rowsAffected == 0)
                return Error.NotFound("location.not_found", $"Локация {locationId} не найдена");
            
            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось обновить название локации {LocationId}", locationId);

            return Error.Failure("location.name.update.failed", "Не удалось обновить название локации");
        }
    }
    
    public async Task<bool> ExistsByName(Name name, CancellationToken cancellationToken)
    {
        return await _dbContext.Locations.AnyAsync(l => l.Name == name, cancellationToken);
    }

    public async Task<bool> ExistsByNameWithoutId(Name name, Guid locationId, CancellationToken cancellationToken)
    {
        return await _dbContext.Locations.AnyAsync(l => l.Name == name && l.Id != locationId, cancellationToken);
    }

    public async Task<bool> AllExist(IReadOnlyList<Guid> locationIds, CancellationToken cancellationToken)
    {
        if (locationIds.Count == 0)
            return true;

        var distinctIds = locationIds.Distinct().ToList();
        
        int count = await _dbContext.Locations.CountAsync(l => distinctIds.Contains(l.Id), cancellationToken);
        return count == distinctIds.Count;
    }
}