using EcoPark.Application.Reservations.Delete;
using EcoPark.Application.Reservations.Get;
using EcoPark.Application.Reservations.Insert;
using EcoPark.Application.Reservations.List;
using EcoPark.Application.Reservations.Models;
using EcoPark.Application.Reservations.Update;
using EcoPark.Application.Reservations.Update.Status;

namespace EcoPark.Presentation.Controllers;

/// <summary>
/// Endpoints para Operações relacionadas a Reservas
/// </summary>
/// <param name="logger"></param>
[Route("[controller]")]
[ApiController]
public class ReservationController(ILogger<ReservationController> logger) : ControllerBase
{
    /// <summary>
    /// Método para listar todas as reservas cadastradas, limitados ao escopo de permissões do funcionario ou do administrador
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de reservas</returns>
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

    /// <summary>
    /// Método para alterar status de uma reserva para "Chegou"
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="reservationCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [HttpPut("arrived")]
    [Authorize(Roles = "System")]
    public async Task<IActionResult> Arrived([FromServices] IHandler<UpdateReservationStatusCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] string reservationCode, CancellationToken cancellationToken)
    {
        UpdateReservationStatusCommand command = new();
        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        command.SetReservationCode(reservationCode);
        command.SetReservationStatus(EReservationStatus.Arrived);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Created(Request.GetDisplayUrl(), result),
            EOperationStatus.Failed => NotFound(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para alterar status de uma reserva para "Cancelado"
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [HttpPut("cancel")]
    [Authorize(Roles = "System")]
    public async Task<IActionResult> Cancel([FromServices] IHandler<UpdateReservationStatusCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] Guid id, CancellationToken cancellationToken)
    {
        UpdateReservationStatusCommand command = new();

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        command.SetReservationId(id);
        command.SetReservationStatus(EReservationStatus.Cancelled);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Created(Request.GetDisplayUrl(), result),
            EOperationStatus.Failed => NotFound(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para buscar uma reserva pelo seu Id, limitado ao escopo de permissões do funcionario ou do administrador
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [ProducesResponseType(typeof(ReservationSimplifiedViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EntityNotFoundValueObject), StatusCodes.Status404NotFound)]
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

        return result is not null ? Ok(result) : NotFound(new EntityNotFoundValueObject($"Client not found"));
    }

    /// <summary>
    /// Método para inserir uma nova reserva
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
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

    /// <summary>
    /// Método para atualizar uma reserva
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
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

    /// <summary>
    /// Método para deletar uma reserva
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [HttpDelete]
    [Authorize(Roles = "PlataformAdministrator, Administrator, Employee, Client")]
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