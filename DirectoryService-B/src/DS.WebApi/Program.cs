using DS.Application;
using DS.Application.Database;
using DS.Infrastructure.Postgresql;
using DS.Infrastructure.Postgresql.Database;
using DS.Infrastructure.Postgresql.Repositories;
using DS.WebApi.Middlewares;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters
            .Add(new System.Text.Json.Serialization.JsonStringEnumConverter())); // глобальный конвертер enum'ов в строки


builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer((schema, context, _) =>
    {
        if (context.JsonTypeInfo.Type.IsEnum)
        {
            schema.Type = "string";
            schema.Format = null;
            schema.Enum = [..Enum.GetNames(context.JsonTypeInfo.Type)
                .Select(name => new Microsoft.OpenApi.Any.OpenApiString(name))];
        }

        return Task.CompletedTask;
    });
});

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

builder.Services.AddScoped<ILocationsRepository, EfCoreLocationsRepository>();
// builder.Services.AddScoped<ILocationsRepository, NpgsqlLocationsRepository>();


builder.Services.AddApplication();


var app = builder.Build();

app.UseExceptionMiddleware();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DirectoryService"));
}

app.MapControllers();

app.Run();