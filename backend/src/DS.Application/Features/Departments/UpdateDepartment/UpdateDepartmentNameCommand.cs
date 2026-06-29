using DS.Application.Interfaces.Abstractions;
using DS.Contracts.Departments.Update;

namespace DS.Application.Features.Departments.UpdateDepartment;

public record UpdateDepartmentNameCommand(Guid Id, UpdateDepartmentNameRequest Request) : ICommand;