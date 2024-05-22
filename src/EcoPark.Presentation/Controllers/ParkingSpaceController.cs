using EcoPark.Application.ParkingSpaces.Delete;
using EcoPark.Application.ParkingSpaces.Get;
using EcoPark.Application.ParkingSpaces.Insert;
using EcoPark.Application.ParkingSpaces.List;
using EcoPark.Application.ParkingSpaces.Models;
using EcoPark.Application.ParkingSpaces.Update;
using EcoPark.Application.ParkingSpaces.Update.Status;

namespace EcoPark.Presentation.Controllers;

/// <summary>
/// Endpoints para Operações relacionadas a Vagas de Estacionamento
/// </summary>
/// <param name="logger"></param>
[Route("[controller]")]
[ApiController]
public class ParkingSpaceController(ILogger<ParkingSpaceController> logger) : ControllerBase
{
    /// <summary>
    /// Método para ocupar uma vaga de estacionamento
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações da Vaga de Estacionamento")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
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
            EOperationStatus.NotFound => NotFound(result),
            EOperationStatus.Failed => BadRequest(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para desocupar uma vaga de estacionamento
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações da Vaga de Estacionamento")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
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
            EOperationStatus.NotFound => NotFound(result),
            EOperationStatus.Failed => BadRequest(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para listar todas as vagas de estacionamento, limitadas pelo escopo de permissão do funcionário ou administrador
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de vagas de estacionamento</returns>
    [Tags("Operações da Vaga de Estacionamento")]
    [ProducesResponseType(typeof(IEnumerable<ParkingSpaceSimplifiedViewModel>), StatusCodes.Status200OK)]
    [HttpPost("list")]
    [Authorize(Roles = "PlatformAdministrator, Administrator, Employee")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListParkingSpacesQuery, IEnumerable<ParkingSpaceSimplifiedViewModel>> handler,
        [FromBody] ListParkingSpacesQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListParkingSpaces with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    /// <summary>
    /// Método para buscar uma vaga de estacionamento pelo seu Id, limitado pelo escopo de permissão do funcionário ou administrador
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Vaga de estacionamento</returns>
    [Tags("Informações da Vaga de Estacionamento")]
    [ProducesResponseType(typeof(ParkingSpaceSimplifiedViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EntityNotFoundValueObject), StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(Roles = "PlatformAdministrator, Administrator, Employee")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetParkingSpaceQuery, ParkingSpaceSimplifiedViewModel> handler,
        [FromQuery] GetParkingSpaceQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(query, cancellationToken);

        return result is not null ? Ok(result) : NotFound(new EntityNotFoundValueObject($"Client not found"));
    }

    /// <summary>
    /// Método para inserir uma nova vaga de estacionamento
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações da Vaga de Estacionamento")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [HttpPost]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertParkingSpaceCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertParkingSpaceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Created(Request.GetDisplayUrl(), result),
            EOperationStatus.Failed => BadRequest(result),
            EOperationStatus.NotFound => NotFound(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para atualizar uma vaga de estacionamento
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="Id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações da Vaga de Estacionamento")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
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
            EOperationStatus.NotFound => NotFound(result),
            EOperationStatus.Failed => BadRequest(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para deletar uma vaga de estacionamento
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações da Vaga de Estacionamento")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
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
            EOperationStatus.NotFound => NotFound(result),
            EOperationStatus.Failed => BadRequest(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }
}