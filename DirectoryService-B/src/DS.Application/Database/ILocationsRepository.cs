using CSharpFunctionalExtensions;
using DS.Domain.Entities;
using Shared.AppFails;

namespace DS.Application.Database;

public interface ILocationsRepository
{
    Task<Result<Guid, Error>> Add(Location location, CancellationToken cancellationToken);
}