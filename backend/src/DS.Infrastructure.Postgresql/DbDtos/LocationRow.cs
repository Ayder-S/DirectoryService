namespace DS.Infrastructure.Postgresql.DbDtos;

public sealed record LocationRow(                                                                                                                                                                       
    Guid Id,                                                                                                                                                                                             
    string Name,                                                                                                                                                                                         
    string Address,                                                                                                                                                                                      
    string Timezone,                                                                                                                                                                                     
    bool IsActive,                                                                                                                                                                                       
    DateTime CreatedAt,                                                                                                                                                                                  
    DateTime UpdatedAt);