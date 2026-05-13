using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared.AppFails;
using Shared.Constants;

namespace DS.Domain.ValueObjects;

public record Identifier
{
    private const string PATTERN = "^[a-z]+(-[a-z]+)*$";
    private static readonly Regex _identifierFormat = new (PATTERN, RegexOptions.Compiled | RegexOptions.CultureInvariant);


    private Identifier(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<Identifier, Error> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation("identifier.is.not.valid", "Идентификатор должен быть заполнен", "identifier");

        string normalized = value.Trim().ToLowerInvariant();

        if (normalized.Length < LengthConstants.Identifier.MIN_LENGTH || normalized.Length > LengthConstants.Identifier.MAX_LENGTH)
            return Error.Validation("identifier.is.not.valid", $"Идентификатор может быть длиной от {LengthConstants.Identifier.MIN_LENGTH} до {LengthConstants.Identifier.MAX_LENGTH} символов", "identifier");

        if (!_identifierFormat.IsMatch(normalized))
            return Error.Validation("identifier.is.not.valid", "Идентификатор может состоять только из латиницы, а также дефиса только в середине идентификатора", "identifier");

        return new Identifier(normalized);
    }

    public static Identifier ReadIdentifier(string value) => new Identifier(value);
}