using DS.Application.Abstractions;
using DS.Application.Commands.Department;
using DS.Contracts.Departments.Create;
using Microsoft.AspNetCore.Mvc;
using Shared.EndpointsResult;

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
    
}