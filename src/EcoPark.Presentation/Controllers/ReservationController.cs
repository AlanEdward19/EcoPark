using EcoPark.Application.Reservations.Delete;
using EcoPark.Application.Reservations.Get;
using EcoPark.Application.Reservations.List;
using EcoPark.Application.Reservations.Models;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class ReservationController(ILogger<ReservationController> logger) : ControllerBase
{
    [HttpPost("list")]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListReservationQuery?, IEnumerable<ReservationSimplifiedViewModel>> handler,
        [FromBody] ListReservationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListReservations with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetReservationQuery, ReservationModel> handler, GetReservationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetReservation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertReservationCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertReservationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertReservation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPatch]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateReservationCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] UpdateReservationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateReservation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpDelete]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteReservationCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteReservationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteReservation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.Status == "Successful")
            return Accepted(Request.GetDisplayUrl(), result);

        if (result.Message == "No Reservation were found with this id")
            return NotFound(result);

        return BadRequest(result);
    }
}