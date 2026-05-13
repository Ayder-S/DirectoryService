using CSharpFunctionalExtensions;
using Shared.AppFails;

namespace DS.Domain.ValueObjects;

public record Depth
{
    private const int DEFAULT_DEPTH = 1;

    private Depth(Path path)
    {
        Value = DEFAULT_DEPTH;

        foreach (char s in path.Value)
        {
            if (s == path.PathSeparator)
                Value++;
        }
    }
    
    private Depth(short value)
    {
        Value = value;
    }

    public short Value { get; }

    public static Result<Depth, Error> Create(Path path)
    {
        if (path is null)
            return Error.Validation("path.is.not.valid", "Путь не может быть пустым");

        return new Depth(path);
    }
    
    public static Depth FromValue(short value) => new(value);
}