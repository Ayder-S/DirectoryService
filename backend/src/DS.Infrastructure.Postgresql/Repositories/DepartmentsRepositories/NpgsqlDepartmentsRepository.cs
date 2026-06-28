using CSharpFunctionalExtensions;
using Dapper;
using DS.Application.Interfaces.Database;
using DS.Domain.Entities;
using DS.Domain.Relation;
using DS.Domain.ValueObjects;
using DS.Infrastructure.Postgresql.Database;
using DS.Infrastructure.Postgresql.DbDtos;
using Microsoft.Extensions.Logging;
using Shared.Kernel.AppFails;
using Path = DS.Domain.ValueObjects.Path;                                                                                                                                                                

namespace DS.Infrastructure.Postgresql.Repositories.DepartmentsRepositories;

public class NpgsqlDepartmentsRepository : IDepartmentsRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgsqlDepartmentsRepository> _logger;

    public NpgsqlDepartmentsRepository(IDbConnectionFactory connectionFactory, ILogger<NpgsqlDepartmentsRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Add(
        Department department,
        IReadOnlyList<DepartmentLocation> relation,
        CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        using var transaction = connection.BeginTransaction();

        try
        {
            const string departmentInsertSql = """
                                               INSERT INTO departments (id, parent_id, name, identifier, path, depth, is_active, created_at, updated_at)
                                               VALUES (@Id, @ParentId, @Name,  @Identifier, @Path, @Depth, @IsActive, @CreatedAt, @UpdatedAt)
                                               """;

            var departmentParameters = new
            {
                Id = department.Id,
                ParentId = department.ParentId,
                Name = department.Name.Value,
                Identifier = department.Identifier.Value,
                Path = department.Path.Value,
                Depth = department.Depth.Value,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt,
            };

            await connection.ExecuteAsync(
                new CommandDefinition(
                    departmentInsertSql,
                    departmentParameters,
                    transaction,
                    cancellationToken: cancellationToken));

            if (relation.Count > 0)
            {
                const string relationInsertSql = """
                                                 INSERT INTO department_locations (id, department_id, location_id)
                                                 VALUES (@DepartmentLocationId, @DepartmentId, @LocationId)
                                                 """;

                var relationParameters = relation.Select(r => new
                {
                    r.DepartmentLocationId, 
                    r.DepartmentId,
                    r.LocationId,
                });

                await connection.ExecuteAsync(
                    new CommandDefinition(
                        relationInsertSql,
                        relationParameters,
                        transaction,
                        cancellationToken: cancellationToken));
            }

            transaction.Commit();
            
            return department.Id;
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            
            _logger.LogError(exception, "Не удалось сохранить подразделение {DepartmentId}", department.Id);

            return Error.Failure("department.add.failed", "Не удалось сохранить подразделение");
        }
    }

    public async Task<Result<Department, Error>> GetById(Guid departmentId, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        try
        {
            const string getByIdSql = """
                                      SELECT id          AS Id,
                                             parent_id   AS ParentId,
                                             name        AS Name,
                                             identifier  AS Identifier,
                                             path        AS Path,
                                             depth       AS Depth, 
                                             is_active   AS IsActive, 
                                             created_at  AS CreatedAt, 
                                             updated_at  AS UpdatedAt
                                      FROM departments
                                      WHERE id = @Id
                                      """;
            
            var row = await connection.QueryFirstOrDefaultAsync<DepartmentRow>(
                new CommandDefinition(
                    getByIdSql, 
                    new { Id = departmentId },
                    cancellationToken: cancellationToken));
            
            if (row is null)
                return Error.NotFound("department.not_found", $"Подразделение {departmentId} не найдено");
            
            var name = Name.ReadName(row.Name);
            var identifier = Identifier.ReadIdentifier(row.Identifier);
            var path = Path.ReadPath(row.Path);
            var depth = Depth.FromValue(row.Depth);

            return Department.RestoreFromDb(
                row.Id,
                name,
                identifier,
                row.ParentId,
                path,
                depth,
                row.IsActive,
                row.CreatedAt,
                row.UpdatedAt);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Не удалось получить департамент {DepartmentId}", departmentId);
            return Error.Failure("department.get.failed", "Не удалось получить подразделение");
        }
    }

    public async Task<UnitResult<Error>> Update(Department department, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        using var transaction = connection.BeginTransaction();

        try
        {
            const string updateDepartmentSql = """
                                               UPDATE departments
                                               SET name = @Name, identifier = @Identifier, depth = @Depth, path = @Path, updated_at = @UpdatedAt
                                               WHERE id = @Id
                                               """;
            
            var parameters = new
            {
                Id = department.Id,
                Name = department.Name.Value,
                Identifier = department.Identifier,
                Depth = department.Depth.Value,
                Path = department.Path.Value,
                UpdatedAt = department.UpdatedAt,
            };
            
            int rowsAffected = await connection.ExecuteAsync(
                new CommandDefinition(
                    updateDepartmentSql,
                    parameters,
                    transaction,
                    cancellationToken: cancellationToken));

            if (rowsAffected == 0)
                return Error.NotFound("department.not_found", $"Подразделение {department.Id} не найдено");
            
            transaction.Commit();
            
            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            
            _logger.LogError(exception, "Не удалось обновить название подразделения {DepartmentId}",  department.Id);
            return Error.Failure("department.name.update.failed", "Не удалось обновить название подразделения");
        }
    }

    public async Task<UnitResult<Error>> UpdateName(Department department, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        using var transaction = connection.BeginTransaction();

        try
        {
            const string updateNameSql = """
                                         UPDATE departments
                                         SET name = @Name, updated_at = @UpdatedAt
                                         WHERE id = @Id
                                         """;

            var parameters = new
            {
                Id = department.Id,
                Name = department.Name.Value,
                UpdatedAt = department.UpdatedAt,
            };

            int rowsAffected = await connection.ExecuteAsync(
                new CommandDefinition(
                    updateNameSql,
                    parameters,
                    transaction,
                    cancellationToken: cancellationToken));

            if (rowsAffected == 0)
                return Error.NotFound("department.not_found", $"Подразделение {department.Id} не найдено");
            
            transaction.Commit();
            
            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            
            _logger.LogError(exception, "Не удалось обновить название подразделения {DepartmentId}",  department.Id);
            return Error.Failure("department.name.update.failed", "Не удалось обновить название подразделения");
        }
    }
}