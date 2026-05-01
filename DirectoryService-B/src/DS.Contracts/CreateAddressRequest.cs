namespace DS.Contracts;

public record CreateAddressRequest(
    string Country,
    string Region,
    string City,
    string? Street,
    string? Building);