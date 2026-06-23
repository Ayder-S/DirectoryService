using CSharpFunctionalExtensions;
using DS.Domain.Entities;
using Shared.AppFails;

namespace DS.Application.Database;

public interface IDepartmentLocationsRepository
{
    Task<UnitResult<Error>> AddRelation(Guid departmentId, Guid locationId, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> DeleteRelation(Guid departmentId, Guid locationId, CancellationToken cancellationToken);
    
    Task<bool> DepLocRelationExists(Guid departmentId, Guid locationId, CancellationToken cancellationToken);
}