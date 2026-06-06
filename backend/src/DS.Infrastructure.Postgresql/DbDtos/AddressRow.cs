namespace DS.Infrastructure.Postgresql.DbDtos;

public sealed record AddressRow(                                                                                                                                                                        
    string Country,                                                                                                                                                                                      
    string Region,                                                                                                                                                                                       
    string City,                                                                                                                                                                                         
    string? Street,                                                                                                                                                                                      
    string? Building);