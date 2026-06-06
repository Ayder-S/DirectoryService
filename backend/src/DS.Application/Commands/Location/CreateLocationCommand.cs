using DS.Application.Abstractions;
using DS.Contracts.Locations.Create;

namespace DS.Application.Commands.Location;

public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;