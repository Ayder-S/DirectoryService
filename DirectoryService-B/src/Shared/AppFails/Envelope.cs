using System.Text.Json.Serialization;

namespace Shared.AppFails;

public record Envelope
{
    public object? Result { get; }
    public ErrorsList? Errors { get; }
    public DateTime GeneratedAt { get; }
    public bool IsError => Errors is not null && Errors.Any();
    
    [JsonConstructor]
    private Envelope(object? result,  ErrorsList? errors)
    {
        Result = result;
        Errors = errors;
        GeneratedAt = DateTime.UtcNow;
    }
    
    public static Envelope Ok(object? result = null) => new(result, null);
    public static Envelope Fail(ErrorsList errors) => new(null, errors);
}

public record Envelope<T>
{
    public T? Result { get; }
    public ErrorsList? Errors { get; }
    public DateTime GeneratedAt { get; }
    public bool IsError => Errors is not null && Errors.Any();
    
    [JsonConstructor]
    private Envelope(T? result,  ErrorsList? errors)
    {
        Result = result;
        Errors = errors;
        GeneratedAt = DateTime.UtcNow;
    }
    
    public static Envelope<T> Ok(T? result = default) => new (result, null);
    public static Envelope<T> Fail(ErrorsList errors) => new (default, errors);
    
}