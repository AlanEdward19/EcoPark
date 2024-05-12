using EcoPark.Application.Clients.Delete;
using EcoPark.Application.Clients.Get;
using EcoPark.Application.Clients.Insert;
using EcoPark.Application.Clients.List;
using EcoPark.Application.Clients.Models;
using EcoPark.Application.Clients.Update;
using EcoPark.Application.Rewards.List.ListUserRewards;
using EcoPark.Application.Rewards.Models;
using EcoPark.Application.Rewards.Update.UseReward;
using static System.Net.Mime.MediaTypeNames;

namespace EcoPark.Presentation.Controllers;
/// <summary>
/// Endpoints para Operações relacionadas a Clientes
/// </summary>
/// <param name="logger"></param>
[Route("[controller]")]
[ApiController]
public class ClientController(ILogger<ClientController> logger) : ControllerBase
{
    /// <summary>
    /// Método para listar todos os clientes cadastrados [Somente donos da plataforma tem acesso]
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de clientes cadastrados</returns>
    [Tags("Informações do Cliente")]
    [HttpPost("list")]
    [Authorize(Roles = "PlataformAdministrator, Administrator, Employee")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListClientsQuery, IEnumerable<ClientSimplifiedViewModel>> handler,
        [FromBody] ListClientsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListClients with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    /// <summary>
    /// Método para buscar um cliente pelo Id
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Cliente Cadastrado</returns>
    [Tags("Informações do Cliente")]
    [ProducesResponseType(typeof(ClientSimplifiedViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EntityNotFoundValueObject), StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(Roles = "PlataformAdministrator, Administrator, Employee")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetClientQuery, ClientSimplifiedViewModel?> handler, 
        [FromQuery] GetClientQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetClient with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(query, cancellationToken);

        return result is not null ? Ok(result) : NotFound(new EntityNotFoundValueObject($"Client not found"));
    }

    /// <summary>
    /// Método para inserir um novo cliente
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Cliente")]
    [HttpPost]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertClientCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] InsertClientCommand command, [FromForm] IFormFile image, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertClient with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        await command.SetImage(image, image.FileName, cancellationToken);

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    /// <summary>
    /// Método para atualizar um cliente
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Cliente")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPatch]
    [Authorize(Roles = "PlataformAdministrator, Client")]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateClientCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] Guid id, [FromBody] UpdateClientCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateClient with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetClientId(id);
        command.SetRequestUserInfo(requestUserInfo);

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
    /// Método para deletar um cliente
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Cliente")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    [Authorize(Roles = "PlataformAdministrator, Client")]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteClientCommand, DatabaseOperationResponseViewModel> handler, 
        [FromQuery] DeleteClientCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteClient with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

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
    /// Método para usar uma recompensa
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Tags("Operações de Recompensas")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPut("useReward")]
    public async Task<IActionResult> UseReward(
        [FromServices] IHandler<UseRewardCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] UseRewardCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UseReward with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Ok(result),
            EOperationStatus.Failed => BadRequest(result),
            EOperationStatus.NotFound => NotFound(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para listar todos as recompensas resgatadas por um usuario
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de recompensas de um usuario</returns>
    [Tags("Informações de Recompensas")]
    [HttpPost("Reward/list")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetRewardsList([FromServices] IHandler<ListUserRewardsQuery, IEnumerable<UserRewardViewModel>> handler,
        [FromBody] ListUserRewardsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListRewards [Client] with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }
}