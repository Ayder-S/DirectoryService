using DS.Application.Abstractions;
using DS.Application.Locations;
using DS.Contracts.Locations.Create;
using DS.Contracts.Locations.Get;
using Microsoft.AspNetCore.Mvc;
using Shared.EndpointsResult;
using Shared.Pagination;

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

    // [HttpGet("locationId:guid")]
    // public async Task<EndpointResult<Guid>> GetById([FromRoute]Guid locationId, CancellationToken cancellationToken)
    // {
    //     
    // }
    [HttpGet("{locationId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid locationId, CancellationToken cancellationToken)
    {
        return Ok();
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
    
    // [HttpPut("{locationId:guid}")]
    // public async Task<EndpointResult<Guid>> Update(
    //     [FromRoute] Guid locationId,
    //     [FromServices] ICommandHandler<Guid, UpdateLocationCommand> handler,
    //     [FromBody] UpdateLocationRequest request,
    //     CancellationToken cancellationToken)
    // {
    //     
    // } позже будет реализация 
    [HttpPut("{locationId:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid locationId, CancellationToken cancellationToken)
    {
        return Ok();
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