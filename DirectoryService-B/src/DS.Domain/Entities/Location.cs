using CSharpFunctionalExtensions;
using DS.Domain.Relation;
using DS.Domain.ValueObjects;
using Shared.AppFails;

namespace DS.Domain.Entities;

public class Location
{
    private readonly List<DepartmentLocation> _departments = [];
    
    private Location() { }

    private Location(Name name, Address address, Timezone timezone)
    {
        Id = Guid.NewGuid();
        Name = name;
        Address = address;
        Timezone = timezone;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public IReadOnlyList<DepartmentLocation> Departments => _departments;

    public Guid Id { get; private set; }

    public Name Name { get; private set; }
    
    public Address Address { get; private set; }
    
    public Timezone Timezone { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }

    public static Result<Location, Error> Create(Name name, Address address, Timezone timezone)
    {
        return new Location(name, address, timezone);
    }
}