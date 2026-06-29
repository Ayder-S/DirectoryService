using DS.Application.Interfaces.Abstractions;
using DS.Contracts.Locations.Create;

namespace DS.Application.Features.Locations.CreateLocation;

public record CreateLocationCommand(CreateLocationRequest Request) : ICommand;