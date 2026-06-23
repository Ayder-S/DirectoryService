using DS.Application.Abstractions;
using DS.Contracts.Departments.Update;

namespace DS.Application.Commands.Department;

public record UpdateDepartmentNameCommand(Guid Id, UpdateDepartmentNameRequest Request) : ICommand;