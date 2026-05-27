using DS.Application.Abstractions;
using DS.Contracts.Locations.Create;

namespace DS.Application.Commands;

public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;