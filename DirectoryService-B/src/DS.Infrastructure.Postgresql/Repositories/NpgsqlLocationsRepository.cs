using System.Data;
using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using DS.Application.Database;
using DS.Domain.Entities;
using DS.Infrastructure.Postgresql.Database;
using Microsoft.Extensions.Logging;
using Shared.AppFails;

namespace DS.Infrastructure.Postgresql.Repositories;

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
            
            await connection.ExecuteAsync(new CommandDefinition(locationInsertSql, parameters, transaction, cancellationToken: cancellationToken));
            
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
}