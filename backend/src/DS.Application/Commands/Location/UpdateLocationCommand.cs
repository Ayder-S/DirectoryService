using DS.Application.Abstractions;
using DS.Contracts.Locations.Update;

namespace DS.Application.Commands.Location;

public record UpdateLocationCommand(Guid Id, UpdateLocationRequest Request) : ICommand;