using System.Data;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using DS.Application.Database;
using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using DS.Infrastructure.Postgresql.Database;
using DS.Infrastructure.Postgresql.DbDtos;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

namespace DS.Infrastructure.Postgresql.Repositories.LocationsRepositories;

public class NpgsqlLocationsRepository : ILocationsRepository
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<NpgsqlLocationsRepository> _logger;

    public NpgsqlLocationsRepository(IDbConnectionFactory connectionFactory, ILogger<NpgsqlLocationsRepository> logger)
    {
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Add(Location location, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        using var transaction = connection.BeginTransaction();

        try
        {
            const string locationInsertSql = """
                                             INSERT INTO locations (id, name, address, timezone, is_active, created_at, updated_at)
                                             VALUES (@Id, @Name, @Address::jsonb, @Timezone, @IsActive, @CreatedAt, @UpdatedAt)
                                             """;

            // Dapper не поддерживает приведение типов ::jsonb прямо в параметре. Нужно передавать jsonb через NpgsqlParameter
            var parameters = new DynamicParameters();
            parameters.Add("Id", location.Id);
            parameters.Add("Name", location.Name.Value);
            parameters.Add("Address", JsonSerializer.Serialize(location.Address), DbType.Object);
            parameters.Add("Timezone", location.Timezone.Value);
            parameters.Add("IsActive", location.IsActive);
            parameters.Add("CreatedAt", location.CreatedAt);
            parameters.Add("UpdatedAt", location.UpdatedAt);

            // await connection.ExecuteAsync(locationInsertSql, parameters); // CancellationToken не передаётся в Dapper:
            await connection.ExecuteAsync(
                new CommandDefinition(
                    locationInsertSql,
                    parameters, 
                    transaction,
                    cancellationToken: cancellationToken));
            
            transaction.Commit();

            return location.Id;
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            
            _logger.LogError(exception, "Не удалось сохранить локацию {LocationId}", location.Id);

            return Error.Failure("location.add.failed", "Не удалось сохранить локацию");
        }
    }

    public async Task<Result<Location, Error>> GetById(Guid locationId, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        try
        {
            const string getByIdSql = """
                                      SELECT id          AS Id,
                                             name        AS Name, 
                                             address     AS Address, 
                                             timezone    AS Timezone,
                                             is_active   AS IsActive,
                                             created_at  AS CreatedAt, 
                                             updated_at  AS UpdatedAt
                                      FROM locations
                                      WHERE id = @Id
                                      """;

            var row = await connection.QueryFirstOrDefaultAsync<LocationRow>(
                new CommandDefinition(
                    getByIdSql,
                    new { Id = locationId },
                    cancellationToken: cancellationToken));

            if (row == null)
                return Error.NotFound("location.not_found", $"Локация {locationId} не найдена");

            var name = Name.ReadName(row.Name);
            var timezone = Timezone.ReadTimezone(row.Timezone);
            
            var addressData = JsonSerializer.Deserialize<AddressRow>(
                row.Address,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            
            var address = Address.Create(
                addressData.Country,
                addressData.Region, 
                addressData.City,
                addressData.Street,
                addressData.Building).Value;

            return Location.RestoreFromDb(
                row.Id,
                name, 
                address,
                timezone, 
                row.IsActive, 
                row.CreatedAt,
                row.UpdatedAt );
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
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        using var transaction = connection.BeginTransaction();

        try
        {
            const string updateLocationSql = """
                                             UPDATE locations
                                             SET name = @Name, address = @Address::jsonb, timezone = @Timezone, updated_at = @UpdatedAt
                                             WHERE id = @Id
                                             """;

            // Dapper не поддерживает приведение типов ::jsonb прямо в параметре. Нужно передавать jsonb через NpgsqlParameter
            var parameters = new DynamicParameters();
            parameters.Add("Id", locationId);
            parameters.Add("Name", name.Value);
            parameters.Add("Address", JsonSerializer.Serialize(address), DbType.Object);
            parameters.Add("Timezone", timezone.Value);
            parameters.Add("UpdatedAt", DateTime.UtcNow);

            // await connection.ExecuteAsync(locationInsertSql, parameters); // CancellationToken не передаётся в Dapper:
            int rowsAffected = await connection.ExecuteAsync(
                new CommandDefinition(
                    updateLocationSql,
                    parameters,
                    cancellationToken: cancellationToken));

            if (rowsAffected == 0)
                return Error.NotFound("location.not_found", $"Локация {locationId} не найдена");

            transaction.Commit();

            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            transaction.Rollback();

            _logger.LogError(exception, "Не удалось обновить локацию {LocationId}", locationId);

            return Error.Failure("location.update.failed", "Не удалось обновить локацию");
        }
    }

    public async Task<UnitResult<Error>> UpdateName(Guid locationId, Name name, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        using var transaction = connection.BeginTransaction();
        
        try
        {
            const string updateNameSql = """
                                             UPDATE locations 
                                             SET name = @Name, updated_at = @UpdatedAt
                                             WHERE id = @Id
                                             """;

            var parameters = new { 
                Id = locationId,
                Name = name.Value,
                UpdatedAt = DateTime.UtcNow,
            };

            // await connection.ExecuteAsync(locationInsertSql, parameters); // CancellationToken не передаётся в Dapper:
            int rowsAffected = await connection.ExecuteAsync(
                new CommandDefinition(
                    updateNameSql,
                    parameters, 
                    cancellationToken: cancellationToken));

            if (rowsAffected == 0)
                return Error.NotFound("location.not_found", $"Локация {locationId} не найдена");
            
            transaction.Commit();
            
            return UnitResult.Success<Error>();
        }
        catch (Exception exception)
        {
            transaction.Rollback();
            
            _logger.LogError(exception, "Не удалось обновить название локации {LocationId}", locationId);

            return Error.Failure("location.name.update.failed", "Не удалось обновить название локации");
        }
    }

    public async Task<bool> ExistsByName(Name name, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string uniqueNameSql = """
                                     SELECT EXISTS(SELECT 1 FROM locations WHERE name = @Name)
                                     """;

        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                uniqueNameSql,
                new { Name = name.Value },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> ExistsByNameWithoutId(Name name, Guid locationId, CancellationToken cancellationToken)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        
        const string uniqueNameWithoutIdSql = """
                                              SELECT EXISTS(SELECT 1 FROM locations WHERE name = @Name AND id <> @Id)
                                              """;

        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                uniqueNameWithoutIdSql,
                new { Name = name.Value, Id = locationId },
                cancellationToken: cancellationToken));
    }

    public async Task<bool> AllExist(IReadOnlyList<Guid> locationIds, CancellationToken cancellationToken)
    {
        if (locationIds.Count == 0)
            return true;
        
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);

        const string allLocationsExist = """
                                         SELECT COUNT(*) FROM locations WHERE id = ANY(@Ids)
                                         """;

        var distinctIds = locationIds.Distinct().ToArray();

        int count = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                allLocationsExist,
                new { Ids = distinctIds },
                cancellationToken: cancellationToken));
        
        return count == distinctIds.Length;
    }
}