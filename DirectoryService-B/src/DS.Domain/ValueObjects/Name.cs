using CSharpFunctionalExtensions;

namespace DS.Domain.ValueObjects;

public record Name
{
    private const int MIN_LENGTH = 3;
    private const int MAX_LENGTH = 150;

    private Name(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<Name> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < MIN_LENGTH || value.Length > MAX_LENGTH)
            return Result.Failure<Name>($"Название должно быть длиной от {MIN_LENGTH} до {MAX_LENGTH} символов");

        string normalized = value.Trim();

        return new Name(normalized);
    }
}