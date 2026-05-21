using DS.Contracts.Locations.Create;

namespace DS.Contracts.Locations.Update;

public record UpdateLocationRequest(
    string Name,
    CreateAddressRequest Address,
    string TimeZone);