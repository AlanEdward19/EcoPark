using EcoPark.Application.Reservations.Delete;
using EcoPark.Application.Reservations.Get;
using EcoPark.Application.Reservations.Insert;
using EcoPark.Application.Reservations.List;
using EcoPark.Application.Reservations.Models;
using EcoPark.Application.Reservations.Update;
using EcoPark.Domain.Commons.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class ReservationController(ILogger<ReservationController> logger) : ControllerBase
{
    [HttpPost("list")]
    [Authorize(Roles = "Administrator, Employee, Client")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListReservationQuery?, IEnumerable<ReservationSimplifiedViewModel>> handler,
        [FromBody] ListReservationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListReservations with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, Employee, Client")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetReservationQuery, ReservationSimplifiedViewModel?> handler,
        [FromQuery] GetReservationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetReservation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(query, cancellationToken);

        return result is null ? NotFound(new { Message = "Reservation not Found" }) : Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertReservationCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertReservationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertReservation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Created(Request.GetDisplayUrl(), result),
            EOperationStatus.Failed => BadRequest(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    [HttpPatch]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateReservationCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] Guid id, [FromBody] UpdateReservationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateReservation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);
        command.SetReservationId(id);

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
    [Authorize(Roles = "Administrator, Employee, Client")]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteReservationCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteReservationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteReservation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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