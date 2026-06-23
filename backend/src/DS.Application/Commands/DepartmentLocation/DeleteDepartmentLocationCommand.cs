using DS.Application.Abstractions;

namespace DS.Application.Commands.DepartmentLocation;

public record DeleteDepartmentLocationCommand(Guid DepartmentId, Guid LocationId) : ICommand;