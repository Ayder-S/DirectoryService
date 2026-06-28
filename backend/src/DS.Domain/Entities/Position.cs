using CSharpFunctionalExtensions;
using DS.Domain.ValueObjects;
using Shared.Kernel.AppFails;

namespace DS.Domain.Entities;

public class Position
{
    private Position() { }

    private Position(Name name, Description description)
    {
        Id = Guid.CreateVersion7();
        Name = name;
        Description = description;
        IsActive = true; 
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
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