using DS.Domain.Entities;

namespace DS.Domain.Relation;

public class DepartmentPosition
{
    private DepartmentPosition() { }

    private DepartmentPosition(Guid departmentId, Guid positionId)
    {
        DepartmentId = departmentId;
        PositionId = positionId;
        
    }
    
    public Guid DepartmentId { get; private set; }

    public Guid PositionId { get; private set; }

    public Department Department { get; private set; } = null!;

    public Position Position { get; private set; } = null!;

    public static DepartmentPosition Create(Guid departmentId, Guid positionId)
    {
        return new DepartmentPosition(departmentId, positionId);
    }
}