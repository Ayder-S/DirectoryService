using DS.Application.Commands.Department;
using DS.Application.Interfaces.Abstractions;
using DS.Contracts.Departments.Create;
using DS.Contracts.Departments.Update;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.EndpointsResult;

namespace DS.Presenters.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentsController : ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromServices] ICommandHandler<Guid, CreateDepartmentCommand> handler,
        [FromBody] CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new CreateDepartmentCommand(request), cancellationToken);
    }

    // [HttpPut("{departmentId:guid}")]
    // public async Task<EndpointResult<Guid>> UpdateDepartment(
    //     [FromRoute] Guid departmentId,
    //     [FromServices] ICommandHandler<Guid, UpdateDepartmentNameCommand> handler,
    //     [FromBody] UpdateDepartmentNameRequest request,
    //     CancellationToken cancellationToken)
    // {
    //     return await handler.Handle(new UpdateDepartmentNameCommand(departmentId, request), cancellationToken);
    // }
    
    [HttpPatch("{departmentId:guid}")]
    public async Task<EndpointResult<Guid>> Update(
        [FromRoute] Guid departmentId,
        [FromServices] ICommandHandler<Guid, UpdateDepartmentNameCommand> handler,
        [FromBody] UpdateDepartmentNameRequest request,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new UpdateDepartmentNameCommand(departmentId, request), cancellationToken);
    }

}