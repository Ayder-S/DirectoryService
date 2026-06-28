using DS.Application.Interfaces.Abstractions;

namespace DS.Application.Commands.DepartmentLocation;

public record CreateDepartmentLocationCommand(Guid DepartmentId, Guid LocationId) : ICommand;