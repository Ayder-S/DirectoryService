using System.Reflection;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Metadata;
using Shared.AppFails;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Shared.EndpointsResult;

public sealed class EndpointResult<TValue> : IResult, IEndpointMetadataProvider
{
    private readonly IResult _result;

    public EndpointResult(Result<TValue, Error> result)
    {
        _result = result.IsSuccess
            ? new SuccessResult<TValue>(result.Value)
            : new ErrorsResult(result.Error);
    }

    public EndpointResult(Result<TValue, ErrorsList> result)
    {
        _result = result.IsSuccess
            ? new SuccessResult<TValue>(result.Value)
            : new ErrorsResult(result.Error);
    }

    public Task ExecuteAsync(HttpContext httpContext) => _result.ExecuteAsync(httpContext);
    
    public static implicit operator EndpointResult<TValue>(Result<TValue, Error> result) => new EndpointResult<TValue>(result); // new(result):
    
    public static implicit operator EndpointResult<TValue>(Result<TValue, ErrorsList> result) => new EndpointResult<TValue>(result);


    public static void PopulateMetadata(MethodInfo method, EndpointBuilder builder)
    {
         ArgumentNullException.ThrowIfNull(method);
         ArgumentNullException.ThrowIfNull(builder);
         
         builder.Metadata.Add(new ProducesResponseTypeMetadata(200, typeof(Envelope<TValue>), ["application/json"]));
         
         builder.Metadata.Add(new ProducesResponseTypeMetadata(400, typeof(Envelope), ["application/json"]));
         builder.Metadata.Add(new ProducesResponseTypeMetadata(401, typeof(Envelope), ["application/json"]));
         builder.Metadata.Add(new ProducesResponseTypeMetadata(403, typeof(Envelope), ["application/json"]));
         builder.Metadata.Add(new ProducesResponseTypeMetadata(404, typeof(Envelope), ["application/json"]));
         builder.Metadata.Add(new ProducesResponseTypeMetadata(409, typeof(Envelope), ["application/json"]));
         builder.Metadata.Add(new ProducesResponseTypeMetadata(500, typeof(Envelope), ["application/json"]));
    }
}