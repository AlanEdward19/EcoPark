using EcoPark.Application.Clients.Delete;
using EcoPark.Application.Clients.Get;
using EcoPark.Application.Clients.Insert;
using EcoPark.Application.Clients.List;
using EcoPark.Application.Clients.Models;
using EcoPark.Application.Clients.Update;
using EcoPark.Domain.Commons.Enums;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class ClientController(ILogger<ClientController> logger) : ControllerBase
{
    [HttpPut("login")]
    public async Task<IActionResult> Login([FromServices] IHandler<LoginQuery, LoginViewModel> handler, [FromBody] LoginQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: Login [Clients] with email: {query.Email}");

        query.SetIsEmployee(false);
        var result = await handler.HandleAsync(query, cancellationToken);

        if(result == null)
            return NotFound(new { Message = "User not found" });

        return Ok(result);
    }

    [HttpPost("list")]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListClientsQuery, IEnumerable<ClientSimplifiedViewModel>> handler,
        [FromBody] ListClientsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListClients with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetClientQuery, ClientSimplifiedViewModel> handler, 
        [FromQuery] GetClientQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetClient with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertClientCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertClientCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertClient with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPatch]
    [Authorize(Roles = "Administrator, Employee, Client")]
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

            EOperationStatus.Failed => BadRequest(result),

            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    [HttpDelete]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteClientCommand, DatabaseOperationResponseViewModel> handler, 
        [FromQuery] DeleteClientCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteClient with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Created(Request.GetDisplayUrl(), result),

            EOperationStatus.Failed => BadRequest(result),

            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }
}