using EcoPark.Application.ParkingSpaces.Delete;
using EcoPark.Application.ParkingSpaces.Get;
using EcoPark.Application.ParkingSpaces.Insert;
using EcoPark.Application.ParkingSpaces.List;
using EcoPark.Application.ParkingSpaces.Models;
using EcoPark.Application.ParkingSpaces.Update;
using EcoPark.Domain.Commons.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class ParkingSpaceController(ILogger<ParkingSpaceController> logger) : ControllerBase
{
    [HttpPut("occupy")]
    [Authorize(Roles = "System")]
    public async Task<IActionResult> Occupy([FromServices] IHandler<UpdateParkingSpaceStatusCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] Guid id, CancellationToken cancellationToken)
    {
        UpdateParkingSpaceStatusCommand command = new();
        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        command.SetParkingSpaceId(id);
        command.SetStatus(true);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Created(Request.GetDisplayUrl(), result),
            EOperationStatus.Failed => NotFound(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    [HttpPut("vacate")]
    [Authorize(Roles = "System")]
    public async Task<IActionResult> Vacate([FromServices] IHandler<UpdateParkingSpaceStatusCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] Guid id, CancellationToken cancellationToken)
    {
        UpdateParkingSpaceStatusCommand command = new();
        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        command.SetParkingSpaceId(id);
        command.SetStatus(false);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Created(Request.GetDisplayUrl(), result),
            EOperationStatus.Failed => NotFound(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }


    [HttpPost("list")]
    [Authorize(Roles = "PlataformAdministrator, Administrator, Employee")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListParkingSpacesQuery, IEnumerable<ParkingSpaceSimplifiedViewModel>> handler,
        [FromBody] ListParkingSpacesQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListParkingSpaces with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(Roles = "PlataformAdministrator, Administrator, Employee")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetParkingSpaceQuery, ParkingSpaceSimplifiedViewModel> handler,
        [FromQuery] GetParkingSpaceQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertParkingSpaceCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertParkingSpaceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPatch]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateParkingSpaceCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] UpdateParkingSpaceCommand command, Guid Id, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        command.SetParkingSpaceId(Id);
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
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteParkingSpaceCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteParkingSpaceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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