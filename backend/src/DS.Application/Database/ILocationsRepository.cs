using CSharpFunctionalExtensions;
using DS.Domain.Entities;
using DS.Domain.ValueObjects;
using Shared.AppFails;

namespace DS.Application.Database;

public interface ILocationsRepository
{
    Task<Result<Guid, Error>> Add(Location location, CancellationToken cancellationToken);
    
    Task<bool> ExistsByName(Name name, CancellationToken cancellationToken);
}