using CSharpFunctionalExtensions;
using DS.Application.Features.DepartmentLocations.CreateRelation;
using DS.Application.Features.DepartmentLocations.DeleteRelation;
using DS.Application.Interfaces.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.EndpointsResult;

namespace DS.Presenters.Controllers;

[ApiController]
[Route("api/departments/{departmentId:guid}/locations")]
public class DepartmentLocationsController : ControllerBase
{
    [HttpPost("{locationId:guid}")]
    public async Task<EndpointResult> Create(
        [FromRoute] Guid departmentId, 
        [FromRoute] Guid locationId, 
        [FromServices] ICommandHandler<CreateDepartmentLocationCommand> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new CreateDepartmentLocationCommand(departmentId, locationId), cancellationToken);
    }
    
    [HttpDelete("{locationId:guid}")]
    public async Task<EndpointResult> Delete(
        [FromRoute] Guid departmentId, 
        [FromRoute] Guid locationId, 
        [FromServices] ICommandHandler<DeleteDepartmentLocationCommand> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new DeleteDepartmentLocationCommand(departmentId, locationId), cancellationToken);
    }
}