using CSharpFunctionalExtensions;
using Shared.Kernel.AppFails;
using Shared.Kernel.Constants;


namespace DS.Domain.ValueObjects;

public record Description
{
    private Description(string? description)
    {
        Value = description;
    }

    public string? Value { get; }


    public static Result<Description, Error> Create(string? description)
    {
        if (description?.Length > LengthConstants.Description.MAX_LENGTH)
            return Error.Validation("description.is.not.valid", "Введено слишком длинное описание", "description");
        return new Description(description?.Trim());
    }

    public static Description ReadDescription(string? value) => new Description(value);
}