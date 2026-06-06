namespace DS.Contracts.Departments.Create;

public record CreateDepartmentRequest(
    string Name, 
    string Identifier,
    Guid? ParentId,
    IReadOnlyList<Guid> LocationIds);