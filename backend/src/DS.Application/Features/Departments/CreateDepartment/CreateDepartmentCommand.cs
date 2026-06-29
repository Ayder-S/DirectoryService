using DS.Application.Interfaces.Abstractions;
using DS.Contracts.Departments.Create;

namespace DS.Application.Features.Departments.CreateDepartment;

public record CreateDepartmentCommand(CreateDepartmentRequest Request) : ICommand;