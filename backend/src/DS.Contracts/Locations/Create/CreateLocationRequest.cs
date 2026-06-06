using System.ComponentModel.DataAnnotations;

namespace DS.Contracts.Locations.Create;

public record CreateLocationRequest(
    string Name, 
    CreateAddressRequest Address, 
    string TimeZone);