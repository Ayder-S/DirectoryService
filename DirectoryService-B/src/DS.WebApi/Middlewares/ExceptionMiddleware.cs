using Shared;
using Shared.AppFails;

namespace DS.WebApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error) = exception switch
        {
            BadHttpRequestException => (StatusCodes.Status400BadRequest,
                Error.Failure("bad.request", "Некорректный запрос")),

            KeyNotFoundException => (StatusCodes.Status404NotFound,
                Error.NotFound("Ресурс / запись не найден(а)", null)),

            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,
                Error.Failure("unauthorized", "Доступ запрещён")),

            _ => (StatusCodes.Status500InternalServerError,
                Error.Unknown("Произошла непредвиденная ошибка"))
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsJsonAsync(Envelope.Fail(error));
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this WebApplication app) 
        => app.UseMiddleware<ExceptionMiddleware>();
}