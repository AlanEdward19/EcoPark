using EcoPark.Application.CarbonEmission.Delete;
using EcoPark.Application.CarbonEmission.Insert;
using EcoPark.Application.CarbonEmission.List;
using EcoPark.Application.CarbonEmission.Models;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class CarbonEmissionController(ILogger<CarbonEmissionController> logger) : ControllerBase
{
    /// <summary>
    /// Método para listar todas as emissões de carbono de um determinado usuário
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Tags("Informações da Emissão de Carbono")]
    [ProducesResponseType(typeof(IEnumerable<CarbonEmissionViewModel>), StatusCodes.Status200OK)]
    [HttpPost("list")]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> ListAsync([FromServices] IHandler<ListCarbonEmissionsQuery, IEnumerable<CarbonEmissionViewModel>> handler,
        [FromBody] ListCarbonEmissionsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListCarbonEmissions with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    /// <summary>
    /// Método para adicionar uma nova emissão de carbono
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="reservationId"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Tags("Operações da Emissão de Carbono")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> AddAsync([FromServices] IHandler<InsertCarbonEmissionCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] Guid reservationId, [FromBody] InsertCarbonEmissionCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertCarbonEmission with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);
        command.SetReservationId(reservationId);

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
    /// Método para deletar uma emissão de carbono
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Tags("Operações da Emissão de Carbono")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpDelete]
    [Authorize(Roles = "System")]
    public async Task<IActionResult> DeleteAsync([FromServices] IHandler<DeleteCarbonEmissionCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteCarbonEmissionCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteCarbonEmission with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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