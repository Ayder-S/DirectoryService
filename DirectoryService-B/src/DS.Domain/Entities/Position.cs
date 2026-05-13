using CSharpFunctionalExtensions;
using DS.Domain.Relation;
using DS.Domain.ValueObjects;
using Shared.AppFails;

namespace DS.Domain.Entities;

public class Position
{
    private readonly List<DepartmentPosition> _departments = [];
    
    private Position() { }

    private Position(Name name, Description description)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = true; 
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public IReadOnlyList<DepartmentPosition> Departments => _departments;

    public Guid Id { get; private set; }

    public Name Name { get; private set; }

    public Description Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public static Result<Position, Error> Create(Name name, Description description)
    {
        return new Position(name, description);
    }
}