using CSharpFunctionalExtensions;
using DS.Domain.Relation;
using DS.Domain.ValueObjects;
using Shared.AppFails;
using Path = DS.Domain.ValueObjects.Path;

namespace DS.Domain.Entities;

public class Department
{
    private readonly List<DepartmentPosition> _positions = [];
    private readonly List<DepartmentLocation> _locations = [];
    
    private Department() { }

    private Department(Name name, Guid? parentId, Identifier identifier, Depth depth, Path path)
    {
        Id = Guid.NewGuid();
        Name = name;
        ParentId = parentId;
        Identifier = identifier;
        Depth = depth; 
        Path = path;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public IReadOnlyList<DepartmentPosition> Positions => _positions;

    public IReadOnlyList<DepartmentLocation> Locations => _locations;

    public Guid Id { get; private set; }

    public Guid? ParentId { get; private set; }

    public Name Name { get; private set; }

    public Identifier Identifier { get; private set; }

    public Path Path { get; private set; }
    
    public Depth Depth { get; private set; } // Чтобы избежать ручного ввода

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }
    
    public static Result<Department, Error> Create(Name name, Identifier identifier, Department? parent = null)
    {
        var pathResult = parent is null ? Path.CreateParent(identifier) : Path.CreateChild(parent.Path, identifier);

        if (pathResult.IsFailure)
            return Result.Failure<Department, Error>(pathResult.Error);

        var depthResult = Depth.Create(pathResult.Value);

        if (depthResult.IsFailure)
            return Result.Failure<Department, Error>(depthResult.Error);
        
        return new Department(name, parent?.Id, identifier, depthResult.Value, pathResult.Value);
    }
}