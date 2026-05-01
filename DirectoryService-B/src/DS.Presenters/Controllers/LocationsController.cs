using DS.Application.Abstractions;
using DS.Application.Database;
using DS.Application.Locations;
using DS.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace DS.Presenters.Controllers;

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] ICommandHandler<Guid, CreateLocationCommand> handler,
        [FromBody] CreateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateLocationCommand(request);
        
        var result = await handler.Handle(command, cancellationToken);
        if(!result.IsSuccess)
            return BadRequest(result.Error);
        
        return Ok(result.Value);
    }
}