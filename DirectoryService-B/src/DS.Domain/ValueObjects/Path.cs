using CSharpFunctionalExtensions;
using Shared.AppFails;

namespace DS.Domain.ValueObjects;

public record Path
{
    private const char SEPARATOR = '/';

    private Path(string value)
    {
        Value = value;
    }

    private Path(Path? parentPath, Identifier identifier)
    {
        if (parentPath == null)
            Value = identifier.Value;
        else
            Value = parentPath.Value + SEPARATOR + identifier.Value;
    }

    public char PathSeparator => SEPARATOR;
    public string Value { get; }

    public static Result<Path, Error> CreateParent(Identifier identifier)
    {
        if (identifier is null)
            return Error.Failure("identifier.is.not.valid", "Идентификатор должен быть заполнен");

        return new Path(null, identifier);
    }

    public static Result<Path, Error> CreateChild(Path? parentPath, Identifier identifier)
    {
        if (parentPath == null)
            return Error.Failure("parentPath.is.not.valid", "Не введен родительский путь");
        
        if (identifier is null)
            return Error.Failure("identifier.is.not.valid", "Идентификатор должен быть заполнен");

        return new Path(parentPath, identifier);
    }

    public static Path ReadPath(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException($"Невалидное значение Path в БД: '{value}'");
    
        return new Path(value);
    }
}