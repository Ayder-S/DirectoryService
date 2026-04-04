using CSharpFunctionalExtensions;

namespace DS.Domain.ValueObjects;

public record Description
{
    private const int MAX_LENGTH = 350;

    private Description(string description)
    {
        Value = description;
    }

    public string Value { get; }


    public static Result<Description> Create(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            return Result.Failure<Description>("Описание не может быть пустым");

        if (description.Length > MAX_LENGTH)
            return Result.Failure<Description>("Введено слишком длинное описание");

        return new Description(description.Trim());
    }
}