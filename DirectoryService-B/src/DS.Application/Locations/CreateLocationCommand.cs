using DS.Application.Abstractions;
using DS.Contracts;

namespace DS.Application.Locations;

public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;