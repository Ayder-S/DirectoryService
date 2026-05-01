using DS.Application;
using DS.Application.Database;
using DS.Application.Locations;
using DS.Infrastructure.Postgresql;
using DS.Infrastructure.Postgresql.Database;
using DS.Infrastructure.Postgresql.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString(nameof(DirectoryServiceDbContext))!;

builder.Services.AddDbContext<DirectoryServiceDbContext>(
    (serviceProvider, options) =>
{
    options.UseNpgsql(connectionString);

    if (builder.Environment.IsDevelopment())
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        options
            .UseLoggerFactory(loggerFactory)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
});

builder.Services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();

// builder.Services.AddScoped<ILocationsRepository, EfCoreLocationsRepository>();
builder.Services.AddScoped<ILocationsRepository, NpgsqlLocationsRepository>();


builder.Services.AddApplication();

builder.Services.AddScoped<CreateLocationHandler>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DirectoryService"));
}

app.MapControllers();

app.Run();