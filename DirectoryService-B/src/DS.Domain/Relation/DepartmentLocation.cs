using DS.Domain.Entities;
using DS.Domain.ValueObjects;

namespace DS.Domain.Relation;

public class DepartmentLocation
{
    private DepartmentLocation() { }
    
    private DepartmentLocation(Guid departmentId, Guid locationId)
    {
        DepartmentId = departmentId;
        LocationId = locationId;
    }
    
    public Guid DepartmentId { get; private set; }
    
    public Guid LocationId { get; private set; }


    public Department Department { get; private set; } = null!;

    public Location Location { get; private set; } = null!;


    public static DepartmentLocation Create(Guid departmentId, Guid locationId)
    {
        return new DepartmentLocation(departmentId, locationId);
    }
}