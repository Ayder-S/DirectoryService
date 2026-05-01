using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DS.Infrastructure.Postgresql.Database;

public class NpgsqlConnectionFactory : IDisposable, IAsyncDisposable, IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;
    
    public NpgsqlConnectionFactory(
        IConfiguration configuration,
        ILoggerFactory loggerFactory)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString(nameof(DirectoryServiceDbContext)));

        dataSourceBuilder.UseLoggerFactory(loggerFactory);
        
        _dataSource = dataSourceBuilder.Build();
    }
    
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        return await _dataSource.OpenConnectionAsync(cancellationToken);
    }

    public void Dispose() => _dataSource.Dispose();

    public async ValueTask DisposeAsync() => await _dataSource.DisposeAsync();
}