using CSharpFunctionalExtensions;
using Dapper;
using DS.Application.Database;
using DS.Domain.Entities;
using DS.Domain.Relation;
using DS.Infrastructure.Postgresql.Database;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

namespace DS.Infrastructure.Postgresql.Repositories.DepartmentLocationsRepositories;

public class NpgsqlDepartmentLocationsRepository : IDepartmentLocationsRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgsqlDepartmentLocationsRepository> _logger;
    
    public NpgsqlDepartmentLocationsRepository(IDbConnectionFactory connectionFactory, ILogger<NpgsqlDepartmentLocationsRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<UnitResult<Error>> AddRelation(
        Guid departmentId, 
        Guid locationId,
        CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        using var transaction = connection.BeginTransaction();
        
        try
        {
            var relation = DepartmentLocation.Create(departmentId, locationId);
            
            const string createRelationSql = """
                                          INSERT INTO department_locations (id, department_id, location_id)
                                          VALUES (@DepartmentLocationId, @DepartmentId, @LocationId)
                                          """;
            
            var relationParameters = new
            {
                relation.DepartmentLocationId,
                relation.DepartmentId,
                relation.LocationId,
            };
            
            await connection.ExecuteAsync(
                new CommandDefinition(
                    createRelationSql,
                    relationParameters,
                    transaction,
                    cancellationToken: cancellationToken));
            
            transaction.Commit();
            
            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            
            _logger.LogError(exception, "Не удалось сохранить связь между Локацией {LocationId} и Подразделением {DepartmentId}",  locationId, departmentId);
            return Error.Failure("relation.add.failed", "Не удалось сохранить связь");
        }
    }

    public async Task<UnitResult<Error>> DeleteRelation(
        Guid departmentId,
        Guid locationId,
        CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        using var transaction = connection.BeginTransaction();

        try
        {
            const string deleteRelation = """
                                               DELETE FROM department_locations
                                               WHERE department_id = @departmentId AND location_id = @locationId
                                               """;

            var parameters = new
            {
                DepartmentId = departmentId,
                LocationId = locationId,
            };

            int rowsAffected = await connection.ExecuteAsync(
                new CommandDefinition(
                    deleteRelation,
                    parameters,
                    transaction,
                    cancellationToken: cancellationToken));

            if (rowsAffected == 0)
                return Error.NotFound("departmentLocations.relation.not_found", $"Связь между подразделением {departmentId} и локацией {locationId} не найдена");
            
            transaction.Commit();
            
            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            
            _logger.LogError(exception, "Не удалось удалить связь между Локацией {LocationId} и Подразделением {DepartmentId}", locationId, departmentId);
            return Error.Failure("departmentLocations.relation.delete.failed", "Не удалось удалить связь");
        }
    }

    public async Task<bool> DepLocRelationExists(Guid departmentId, Guid locationId, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string relationExistsSql = """
                                         SELECT EXISTS(SELECT 1 FROM department_locations
                                         WHERE department_id = @departmentId AND location_id = @locationId)
                                         """;

        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                relationExistsSql,
                new { DepartmentId = departmentId, LocationId = locationId },
                cancellationToken: cancellationToken));
    }
}