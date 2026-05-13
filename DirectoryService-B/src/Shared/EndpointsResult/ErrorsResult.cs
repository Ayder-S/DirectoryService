using Microsoft.AspNetCore.Http;
using Shared.AppFails;

namespace Shared.EndpointsResult;

public sealed class ErrorsResult : IResult
{
    private readonly ErrorsList _errors;

    public ErrorsResult(Error error)
    {
        _errors = error.ToErrors();
    }

    public ErrorsResult(ErrorsList errors)
    {
        _errors = errors;
    }

    public Task ExecuteAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        if (_errors.Count == 0)
        {
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return httpContext.Response.WriteAsJsonAsync(Envelope.Fail(_errors));
        }

        var distinctErrorTypes = _errors
            .Select(e => e.ErrorType)
            .Distinct()
            .ToList();

        int statusCode = distinctErrorTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCodeFromErrorType(distinctErrorTypes.First());
        
        var envelope = Envelope.Fail(_errors);
        httpContext.Response.StatusCode = statusCode;
        
        return httpContext.Response.WriteAsJsonAsync(envelope);
    }

    private static int GetStatusCodeFromErrorType(ErrorType errorType)
        => errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError,
        };
}