using CSharpFunctionalExtensions;
using DS.Application.Interfaces.Database;
using DS.Domain.Entities;
using DS.Domain.Relation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Kernel.AppFails;

namespace DS.Infrastructure.Postgresql.Repositories.DepartmentLocationsRepositories;

public class EfCoreDepartmentLocationsRepository : IDepartmentLocationsRepository
{
    private readonly DirectoryServiceDbContext _dbContext;
    private readonly ILogger<EfCoreDepartmentLocationsRepository> _logger;
    
    public EfCoreDepartmentLocationsRepository(DirectoryServiceDbContext dbContext, ILogger<EfCoreDepartmentLocationsRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<UnitResult<Error>> AddRelation(
        Guid departmentId, 
        Guid locationId, 
        CancellationToken cancellationToken)
    {
        try
        {
            var relation = DepartmentLocation.Create(departmentId, locationId);
            
            await _dbContext.DepartmentLocations.AddAsync(relation, cancellationToken);
            
            await _dbContext.SaveChangesAsync(cancellationToken);

            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось сохранить связь между Локацией {LocationId} и Подразделением {DepartmentId}",  locationId, departmentId);
            return Error.Failure("relation.add.failed", "Не удалось сохранить связь");
        }
    }

    public async Task<UnitResult<Error>> DeleteRelation(
        Guid departmentId, 
        Guid locationId, 
        CancellationToken cancellationToken)
    {
        try
        {
            int rowsAffected = await _dbContext.DepartmentLocations
                .Where(dl => dl.DepartmentId == departmentId && dl.LocationId == locationId)
                .ExecuteDeleteAsync(cancellationToken);

            if (rowsAffected == 0)
                return Error.NotFound("departmentLocations.relation.not_found", $"Связь между подразделением {departmentId} и локацией {locationId} не найдена");
            
            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось удалить связь между Локацией {LocationId} и Подразделением {DepartmentId}", locationId, departmentId);
            return Error.Failure("departmentLocations.relation.delete.failed", "Не удалось удалить связь");
        }
    }

    public async Task<bool> DepLocRelationExists(Guid departmentId, Guid locationId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.DepartmentLocations.AnyAsync(dl => dl.DepartmentId == departmentId && dl.LocationId == locationId, cancellationToken);
    }
}