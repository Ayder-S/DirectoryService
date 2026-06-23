using CSharpFunctionalExtensions;
using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using Shared.AppFails;

namespace DS.Application.Database;

public interface ILocationsRepository
{
    Task<Result<Guid, Error>> Add(Location location, CancellationToken cancellationToken);
    
    Task<Result<Location, Error>> GetById(Guid locationId, CancellationToken cancellationToken);

    Task<UnitResult<Error>> Update(Location location, CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> UpdateName(Location location, CancellationToken cancellationToken);
    
    Task<bool> ExistsByName(Name name, CancellationToken cancellationToken);
   
    Task<bool> ExistsByNameWithoutId(Name name, Guid locationId, CancellationToken cancellationToken);
    
    Task<bool> AllExist(IReadOnlyList<Guid> locationIds, CancellationToken cancellationToken);
}