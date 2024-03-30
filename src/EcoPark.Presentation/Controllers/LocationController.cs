using EcoPark.Application.Locations.Delete;
using EcoPark.Application.Locations.Get;
using EcoPark.Application.Locations.List;
using EcoPark.Application.Locations.Models;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class LocationController(ILogger<LocationController> logger) : ControllerBase
{
    [HttpPost("list")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListLocationQuery, IEnumerable<LocationSimplifiedViewModel>> handler,
        [FromBody] ListLocationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListLocations with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetLocationQuery, LocationSimplifiedViewModel> handler, 
        [FromQuery] GetLocationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertLocationCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertLocationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateLocationCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] UpdateLocationCommand command, Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        command.SetLocationId(id);
        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteLocationCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteLocationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.Status == "Successful")
            return Accepted(Request.GetDisplayUrl(), result);

        if (result.Message == "No Location were found with this id")
            return NotFound(result);

        return BadRequest(result);
    }
}