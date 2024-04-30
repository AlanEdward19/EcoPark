using EcoPark.Application.Locations.Delete;
using EcoPark.Application.Locations.Get;
using EcoPark.Application.Locations.Insert;
using EcoPark.Application.Locations.List;
using EcoPark.Application.Locations.Models;
using EcoPark.Application.Locations.Update;
using EcoPark.Domain.Commons.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class LocationController(ILogger<LocationController> logger) : ControllerBase
{
    [HttpPost("list")]
    [Authorize(Roles = "PlataformAdministrator, Administrator, Employee, Client")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListLocationQuery, IEnumerable<LocationSimplifiedViewModel>> handler,
        [FromBody] ListLocationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListLocations with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(Roles = "PlataformAdministrator, Administrator, Employee")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetLocationQuery, LocationSimplifiedViewModel> handler, 
        [FromQuery] GetLocationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertLocationCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertLocationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPatch]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateLocationCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] UpdateLocationCommand command, Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);
        command.SetLocationId(id);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Created(Request.GetDisplayUrl(), result),
            EOperationStatus.Failed => NotFound(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    [HttpDelete]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteLocationCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteLocationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Accepted(Request.GetDisplayUrl(), result),
            EOperationStatus.Failed => NotFound(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }
}