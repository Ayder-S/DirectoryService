using DS.Application.Interfaces.Abstractions;
using DS.Contracts.Locations.Update;

namespace DS.Application.Features.Locations.UpdateLocationName;

public record UpdateLocationNameCommand(Guid Id, UpdateLocationNameRequest Request) : ICommand;
