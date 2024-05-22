using EcoPark.Application.Rewards.Delete;
using EcoPark.Application.Rewards.Get;
using EcoPark.Application.Rewards.Insert;
using EcoPark.Application.Rewards.Insert.RedeemReward;
using EcoPark.Application.Rewards.List;
using EcoPark.Application.Rewards.Models;
using EcoPark.Application.Rewards.Update;

namespace EcoPark.Presentation.Controllers;

/// <summary>
/// Endpoints para Operações relacionadas a Recompensas
/// </summary>
/// <param name="logger"></param>
[Route("[controller]")]
[ApiController]
public class RewardController(ILogger<RewardController> logger) : ControllerBase
{
    /// <summary>
    /// Método para listar todas as recompensas cadastradas, podendo filtrar por localização e lista de ids
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de recompensas</returns>
    [Tags("Informações de Recompensas")]
    [ProducesResponseType(typeof(IEnumerable<RewardViewModel>), StatusCodes.Status200OK)]
    [HttpPost("list")]
    [Authorize(Roles = "Administrator, Employee, Client, PlatformAdministrator")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListRewardsQuery, IEnumerable<RewardViewModel>> handler,
        [FromBody] ListRewardsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListRewards with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    /// <summary>
    /// Método para buscar uma recompensa pelo seu Id
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Informações da Recompensa</returns>
    [Tags("Informações de Recompensas")]
    [ProducesResponseType(typeof(RewardViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EntityNotFoundValueObject), StatusCodes.Status404NotFound)]
    [HttpGet]
    [Authorize(Roles = "Administrator, Employee, Client, PlatformAdministrator")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetRewardQuery, RewardViewModel?> handler,
        [FromQuery] GetRewardQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetReward with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(query, cancellationToken);

        return result is not null ? Ok(result) : NotFound(new EntityNotFoundValueObject($"Reward not found"));
    }

    /// <summary>
    /// Método para resgatar uma recompensa
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações de Recompensas")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPost("redeem")]
    public async Task<IActionResult> RedeemReward(
        [FromServices] IHandler<RedeemRewardCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] RedeemRewardCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
                       $"Method Call: RedeemReward with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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
    /// Método para inserir uma nova recompensa
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="image"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações de Recompensas")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPost]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertRewardCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] InsertRewardCommand command, [FromForm] IFormFile image, CancellationToken cancellationToken)
    {
        logger.LogInformation(
                       $"Method Call: InsertReward with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);
        await command.SetImage(image, image.FileName, cancellationToken);

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
    /// Método para atualizar uma recompensa
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="image"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações de Recompensas")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPatch]
    public async Task<IActionResult> Update(
        [FromServices] IHandler<UpdateRewardCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] UpdateRewardCommand command, [FromForm] IFormFile? image, CancellationToken cancellationToken)
    {
        logger.LogInformation(
                       $"Method Call: UpdateReward with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);
        await command.SetImage(image, image?.FileName, cancellationToken);

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
    /// Método para deletar uma recompensa
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações de Recompensas")]
    [HttpDelete]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteRewardCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteRewardCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteReward with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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