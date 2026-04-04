using CSharpFunctionalExtensions;

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

    public short Value { get; }

    public static Result<Depth> Create(Path path)
    {
        if (path is null)
            return Result.Failure<Depth>("Путь не может быть пустым");

        return new Depth(path);
    }
}