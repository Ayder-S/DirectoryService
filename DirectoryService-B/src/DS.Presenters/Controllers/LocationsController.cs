using DS.Application.Abstractions;
using DS.Application.Locations;
using DS.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.AppFails;
using Shared.EndpointsResult;

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
}