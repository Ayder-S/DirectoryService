using CSharpFunctionalExtensions;
using DS.Domain.Entities;
using DS.Domain.Relation;
using Shared.AppFails;

namespace DS.Application.Database;

public interface IDepartmentsRepository
{
    Task<Result<Guid, Error>> Add(Department department, IReadOnlyList<DepartmentLocation> relation, CancellationToken cancellationToken);
    
    Task<Result<Department, Error>> GetById(Guid departmentId, CancellationToken cancellationToken);
}