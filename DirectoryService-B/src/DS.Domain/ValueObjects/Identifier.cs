using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;

namespace DS.Domain.ValueObjects;

public record Identifier
{
    // Сократил диапазон чтобы идентификатор был покороче
    private const int MIN_LENGTH = 2;
    private const int MAX_LENGTH = 35;

    private const string PATTERN = "^[a-z]+(-[a-z]+)*$";
    private static readonly Regex _identifierFormat = new (PATTERN, RegexOptions.Compiled | RegexOptions.CultureInvariant);


    private Identifier(string value)
    {
        Value = value;
    }

    public string Value { get; }


    public static Result<Identifier> Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result.Failure<Identifier>("Идентификатор должен быть заполнен");

        string normalized = value.Trim().ToLowerInvariant();

        if (normalized.Length < MIN_LENGTH || normalized.Length > MAX_LENGTH)
            return Result.Failure<Identifier>($"Идентификатор может быть длиной от {MIN_LENGTH} до {MAX_LENGTH} символов");

        if (!_identifierFormat.IsMatch(normalized))
            return Result.Failure<Identifier>("Идентификатор может состоять только из латиницы , а также дефиса только в середине идентификатора");

        return new Identifier(normalized);
    }
}