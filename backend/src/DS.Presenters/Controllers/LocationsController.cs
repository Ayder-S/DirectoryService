using DS.Application.Features.Locations.CreateLocation;
using DS.Application.Features.Locations.UpdateLocation;
using DS.Application.Features.Locations.UpdateLocationName;
using DS.Application.Interfaces.Abstractions;
using DS.Contracts.Locations.Create;
using DS.Contracts.Locations.Get;
using DS.Contracts.Locations.Update;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.EndpointsResult;

namespace DS.Presenters.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromServices] ICommandHandler<Guid, CreateLocationCommand> handler,
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new CreateLocationCommand(request), cancellationToken);
    }
    
    // [HttpGet]
    // public async Task<EndpointResult<PagedResult<GetLocationsRequest>>> Get(
    //     [FromServices] IQueryHandler<PagedResult<GetLocationsRequest>, GetLocationsQuery> handler,
    //     [FromQuery] GetLocationsRequest request, 
    //     CancellationToken cancellationToken)
    // {
    //     return await handler.Handle(new GetLocationsQuery(request), cancellationToken);
    // } позже будет реализация 
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetLocationsRequest request, CancellationToken cancellationToken)
    {
        return Ok();
    }
    
    [HttpPatch("{locationId:guid}")]
    public async Task<EndpointResult<Guid>> UpdateLocation(
        [FromRoute] Guid locationId,
        [FromServices] ICommandHandler<Guid, UpdateLocationCommand> handler,
        [FromBody] UpdateLocationRequest request,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new UpdateLocationCommand(locationId, request), cancellationToken);
    }
    
    [HttpPatch("{locationId:guid}/name")]
    public async Task<EndpointResult<Guid>> Update(
        [FromRoute] Guid locationId,
        [FromServices] ICommandHandler<Guid, UpdateLocationNameCommand> handler,
        [FromBody] UpdateLocationNameRequest request,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(new UpdateLocationNameCommand(locationId, request), cancellationToken);
    }

    // [HttpDelete("{locationId:guid}")]
    // public async Task<EndpointResult<Guid>> Delete([FromRoute] Guid locationId, CancellationToken cancellationToken)
    // {
    //     
    // }
    [HttpDelete("{locationId:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid locationId, CancellationToken cancellationToken)
    {
        return Ok();
    }
}