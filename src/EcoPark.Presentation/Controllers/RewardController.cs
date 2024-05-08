using EcoPark.Application.Rewards.Insert;
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
    /// Método para inserir uma nova recompensa
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="image"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
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
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

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
            EOperationStatus.Failed => NotFound(result),
            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }
}