using System.Text.Json.Serialization;

namespace Shared.AppFails;

public record Error
{
    public string Code { get; }
    public string Message { get; }
    public ErrorType ErrorType { get; }
    public string? InvalidField { get; }
    
    private Error(string code, string message, ErrorType errorType, string? invalidField = null)
    {
        Code = code;
        Message = message;
        ErrorType = errorType;
        InvalidField = invalidField;
    }

    public static readonly Error None 
        = new(string.Empty, string.Empty, ErrorType.None, null);
    
    public static Error Unknown(string message)
        => new("error.unknown", message, ErrorType.Unknown);
    
    public static Error NotFound(string message, Guid? id, string code = "record.not.found") 
        => new(code, message, ErrorType.NotFound);
    
    public static Error Validation(string? code, string message, string? invalidField = null) 
        => new(code ?? "record.is.not.valid", message, ErrorType.Validation, invalidField);
    
    public static Error Failure(string? code, string message) 
        => new (code ?? "failure", message, ErrorType.Failure);
    
    public static Error Conflict(string? code, string message) 
        => new (code ?? "conflict", message, ErrorType.Conflict);

    public ErrorsList ToErrors() => this;
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ErrorType
{
    /// <summary>
    /// неизвестная/пустая ошибка
    /// </summary>
    None,
    
    /// <summary>
    /// непредвиденная ошибка
    /// </summary>
    Unknown,
    
    /// <summary>
    /// ошибка - ничего не найдено
    /// </summary>
    NotFound,
    
    /// <summary>
    /// ошибка выполнения
    /// </summary>
    Failure,
    
    /// <summary>
    /// ошибка связанная с валидацией
    /// </summary>
    Validation,
    
    /// <summary>
    /// ошибка конфликт
    /// </summary>
    Conflict,
}