namespace DS.Contracts.Locations.Create;

public record CreateAddressRequest(
    string Country,
    string Region,
    string City,
    string? Street,
    string? Building);