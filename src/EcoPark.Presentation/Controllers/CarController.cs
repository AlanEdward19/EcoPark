using EcoPark.Application.Cars.Delete;
using EcoPark.Application.Cars.Get;
using EcoPark.Application.Cars.Insert;
using EcoPark.Application.Cars.List;
using EcoPark.Application.Cars.Models;
using EcoPark.Application.Cars.Update;
using EcoPark.Domain.Commons.Enums;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Client")]
public class CarController(ILogger<CarController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetCarQuery, CarViewModel?> handler,
        [FromQuery] GetCarQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetCar with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(query, cancellationToken);

        return result is not null ? Ok(result) : NotFound(new { Message = "Car not Found" });
    }

    [HttpPost("list")]
    public async Task<IActionResult> GetList(
        [FromServices] IHandler<ListCarQuery, IEnumerable<CarViewModel>> handler,
        [FromBody] ListCarQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListCars with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Insert(
        [FromServices] IHandler<InsertCarCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertCarCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertCar with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPatch]
    public async Task<IActionResult> Update(
        [FromServices] IHandler<UpdateCarCommand, DatabaseOperationResponseViewModel> handler, [FromQuery] Guid id,
        [FromBody] UpdateCarCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateCar with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);
        command.SetCarId(id);

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
    public async Task<IActionResult> Delete(
        [FromServices] IHandler<DeleteCarCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteCarCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteCar with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

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
