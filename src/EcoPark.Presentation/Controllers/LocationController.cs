using EcoPark.Application.Locations.Delete;
using EcoPark.Application.Locations.Get;
using EcoPark.Application.Locations.Insert;
using EcoPark.Application.Locations.List;
using EcoPark.Application.Locations.Models;
using EcoPark.Application.Locations.Update;

namespace EcoPark.Presentation.Controllers;

/// <summary>
/// Endpoints para Operações relacionadas a Localizações
/// </summary>
/// <param name="logger"></param>
[Route("[controller]")]
[ApiController]
public class LocationController(ILogger<LocationController> logger) : ControllerBase
{
    /// <summary>
    /// Método para listar todas as localizações cadastradas que estejam no escopo de permissões do funcionario ou do administrador
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de localizações</returns>
    [Tags("Informações da Localização")]
    [HttpPost("list")]
    [Authorize(Roles = "PlatformAdministrator, Administrator, Employee, Client")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListLocationQuery, IEnumerable<LocationSimplifiedViewModel>> handler,
        [FromBody] ListLocationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListLocations with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    /// <summary>
    /// Método para buscar uma localização pelo Id que esteja no escopo de permissões do funcionario ou do administrador
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Localização</returns>
    [Tags("Informações da Localização")]
    [ProducesResponseType(typeof(LocationSimplifiedViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EntityNotFoundValueObject), StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(Roles = "PlatformAdministrator, Administrator, Employee")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetLocationQuery, LocationSimplifiedViewModel> handler, 
        [FromQuery] GetLocationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(query, cancellationToken);

        return result is not null ? Ok(result) : NotFound(new EntityNotFoundValueObject($"Location not found"));
    }

    /// <summary>
    /// Método para inserir uma nova localização
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações da Localização")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertLocationCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertLocationCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertLocation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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
    /// Método para atualizar uma localização
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações da Localização")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
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
            EOperationStatus.NotFound => NotFound(result),
            EOperationStatus.Failed => BadRequest(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para deletar uma localização
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações da Localização")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
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
            EOperationStatus.NotFound => NotFound(result),
            EOperationStatus.Failed => BadRequest(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }
}