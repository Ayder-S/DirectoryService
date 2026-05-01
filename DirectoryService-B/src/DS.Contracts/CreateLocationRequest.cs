namespace DS.Contracts;

public record CreateLocationRequest(
    string Name, 
    CreateAddressRequest Address, 
    string Timezone);