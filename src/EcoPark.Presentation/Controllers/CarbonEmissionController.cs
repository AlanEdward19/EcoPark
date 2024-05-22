using EcoPark.Application.CarbonEmission.Delete;
using EcoPark.Application.CarbonEmission.Insert;
using EcoPark.Application.CarbonEmission.List;
using EcoPark.Application.CarbonEmission.Models;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class CarbonEmissionController(ILogger<CarbonEmissionController> logger) : ControllerBase
{
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

    [HttpDelete]
    [Authorize(Roles = "System")]
    public async Task<IActionResult> DeleteAsync([FromServices] IHandler<DeleteCarbonEmissionCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] DeleteCarbonEmissionCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteCarbonEmission with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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
}