using System.Data;

namespace DS.Infrastructure.Postgresql.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}