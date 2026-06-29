using DS.Application.Interfaces.Abstractions;
using DS.Contracts.Locations.Update;

namespace DS.Application.Features.Locations.UpdateLocation;

public record UpdateLocationCommand(Guid Id, UpdateLocationRequest Request) : ICommand;