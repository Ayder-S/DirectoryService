using DS.Application.Abstractions;
using DS.Contracts.Locations.Create;

namespace DS.Application.Locations;

public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;