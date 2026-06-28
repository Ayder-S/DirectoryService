using DS.Application.Interfaces.Abstractions;
using DS.Contracts.Locations.Update;

namespace DS.Application.Commands.Location;

public record UpdateLocationNameCommand(Guid Id, UpdateLocationNameRequest Request) : ICommand;
