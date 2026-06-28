using CSharpFunctionalExtensions;
using DS.Domain.Entities;
using DS.Domain.Relation;
using Shared.Kernel.AppFails;

namespace DS.Application.Interfaces.Database;

public interface IDepartmentsRepository
{
    Task<Result<Guid, Error>> Add(
        Department department,
        IReadOnlyList<DepartmentLocation> relation, 
        CancellationToken cancellationToken);
    
    Task<Result<Department, Error>> GetById(Guid departmentId, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> Update(Department department, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> UpdateName(Department department, CancellationToken cancellationToken);

}