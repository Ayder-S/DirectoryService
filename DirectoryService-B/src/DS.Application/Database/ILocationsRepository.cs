using CSharpFunctionalExtensions;
using DS.Domain.Entities;

namespace DS.Application.Database;

public interface ILocationsRepository
{
    Task<Result<Guid>> Add(Location location, CancellationToken cancellationToken);
}