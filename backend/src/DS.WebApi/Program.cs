using System.Globalization;
using DS.Application;
using DS.Application.Interfaces.Database;
using DS.Infrastructure.Postgresql;
using DS.Infrastructure.Postgresql.Database;
using DS.Infrastructure.Postgresql.Repositories.DepartmentLocationsRepositories;
using DS.Infrastructure.Postgresql.Repositories.DepartmentsRepositories;
using DS.Infrastructure.Postgresql.Repositories.LocationsRepositories;
using DS.WebApi.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using Shared.Observability;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
    .CreateBootstrapLogger();

try
{
    Log.Information("Запуск приложения."); 
    
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddSerilogLogger(builder.Configuration);

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.Converters
                .Add(new System.Text.Json.Serialization.
                    JsonStringEnumConverter())); // глобальный конвертер enum'ов в строки

    Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true; // dapper убирает подчёркивания и сравнивает имена без учёта регистра

    builder.Services.AddOpenApi(options =>
    {
        options.AddSchemaTransformer((schema, context, _) =>
        {
            if (context.JsonTypeInfo.Type.IsEnum)
            {
                schema.Type = "string";
                schema.Format = null;
                schema.Enum =
                [
                    ..Enum.GetNames(context.JsonTypeInfo.Type)
                        .Select(name => new Microsoft.OpenApi.Any.OpenApiString(name))
                ];
            }

            return Task.CompletedTask;
        });
    });

    string connectionString = builder.Configuration.GetConnectionString(nameof(DirectoryServiceDbContext))!;

    builder.Services.AddDbContext<DirectoryServiceDbContext>((serviceProvider, options) =>
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

// builder.Services.AddScoped<IDepartmentsRepository, EfCoreDepartmentsRepository>();
    builder.Services.AddScoped<IDepartmentsRepository, NpgsqlDepartmentsRepository>();

// builder.Services.AddScoped<IDepartmentLocationsRepository, EfCoreDepartmentLocationsRepository>();
    builder.Services.AddScoped<IDepartmentLocationsRepository, NpgsqlDepartmentLocationsRepository>();

    builder.Services.AddApplication();

    builder.Services.Configure<ApiBehaviorOptions>(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    }); // чтобы запрос при невалтдных параметрах доходил до моих хендлеров и валидировался поими методами

    var app = builder.Build();
    
    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestId", httpContext.TraceIdentifier);
        };
    });

    app.UseExceptionMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options => options
            .WithTitle("DirectoryService")
            .WithOpenApiRoutePattern("/openapi/v1.json"));
    }

    app.MapControllers();

    app.MapGet("/api/health", () => Results.Ok(new { status = "healthy" }));

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Непредвиденная ошибка в приложении");
}
finally
{
    Log.CloseAndFlush();
}