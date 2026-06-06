namespace DS.Infrastructure.Postgresql.DbDtos;

public sealed class DepartmentRow                                                                                                                             
{                                                                                                                                                             
    public Guid Id { get; init; }                                                                                                                             

    public Guid? ParentId { get; init; } // Dapper не делает автоконверсию Guid → Guid? изза чего использую класс с init сеттарми.                                                                                                                 

    public string Name { get; init; } = null!;                                                                                                                

    public string Identifier { get; init; } = null!;                                                                                                          

    public string Path { get; init; } = null!;                                                                                                                

    public short Depth { get; init; }                                                                                                                         

    public bool IsActive { get; init; }                                                                                                                       

    public DateTime CreatedAt { get; init; }                                                                                                                  

    public DateTime UpdatedAt { get; init; }                                                                                                                  
} 