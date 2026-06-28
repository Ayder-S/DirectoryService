using DS.Application.Interfaces.Abstractions;
using DS.Contracts.Departments.Create;

namespace DS.Application.Commands.Department;

public record CreateDepartmentCommand(CreateDepartmentRequest Request) : ICommand;