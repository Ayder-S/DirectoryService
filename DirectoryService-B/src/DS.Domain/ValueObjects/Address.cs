using CSharpFunctionalExtensions;
using Shared.AppFails;
using Shared.Constants;

namespace DS.Domain.ValueObjects;

public record Address
{
    private Address(string country, string region, string city, string? street, string? building)
    {
        Country = country;
        Region = region;
        City = city;
        Street = street;
        Building = building;
    }

    public string Country { get; }
    public string Region { get; }
    public string City { get; }
    public string? Street { get; }
    public string? Building { get; }

    public static Result<Address, Error> Create(
        string country, 
        string region, 
        string city, 
        string? street, 
        string? building)
    {
        if (string.IsNullOrWhiteSpace(country))
            return Error.Validation("country.is.required", "Страна обязательна", "address.country");

        if (string.IsNullOrWhiteSpace(region))
            return Error.Validation("region.is.required", "Регион обязателен", "address.region");

        if (string.IsNullOrWhiteSpace(city))
            return Error.Validation("city.is.required", "Город обязателен", "address.city");

        if (country.Length > LengthConstants.Address.MAX_LENGTH)
            return Error.Validation("country.is.not.valid", "Слишком длинное название страны", "address.country");

        if (region.Length > LengthConstants.Address.MAX_LENGTH)
            return Error.Validation("region.is.not.valid", "Слишком длинное название региона или области", "address.region");

        if (city.Length > LengthConstants.Address.MAX_LENGTH)
            return Error.Validation("city.is.not.valid", "Слишком длинное название города", "address.city");

        if (street?.Length > LengthConstants.Address.MAX_LENGTH)
            return Error.Validation("street.is.not.valid", "Слишком длинная улица", "address.street");

        if (building?.Length > LengthConstants.Address.MAX_LENGTH)
            return Error.Validation("building.is.not.valid", "Слишком длинный номер здания", "address.building");

        country = country.Trim();
        city = city.Trim();
        region = region.Trim();
        street = street?.Trim();
        building = building?.Trim();

        return new Address(country, region, city, street, building);
    }
}