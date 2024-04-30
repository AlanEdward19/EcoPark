using EcoPark.Application.Employees.Delete;
using EcoPark.Application.Employees.Get;
using EcoPark.Application.Employees.Insert;
using EcoPark.Application.Employees.List;
using EcoPark.Application.Employees.Models;
using EcoPark.Application.Employees.Update;
using EcoPark.Domain.Commons.Enums;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployeeController(ILogger<EmployeeController> logger) : ControllerBase
{
    [HttpPost("list")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListEmployeesQuery, IEnumerable<EmployeeViewModel>> handler,
        [FromBody] ListEmployeesQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListEmployees with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetEmployeeQuery, EmployeeViewModel?> handler, 
        [FromQuery] GetEmployeeQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetEmployee with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(query, cancellationToken);

        if (result == null)
            return NotFound(new {Message = "Employee not Found"});

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "PlataformAdministrator, Administrator")]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertEmployeeCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertEmployeeCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertEmployee with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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

    [HttpPatch]
    [Authorize(Roles = "Administrator, Employee")]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateEmployeeCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] UpdateEmployeeCommand command, Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateEmployee with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetEmployeeId(id);
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
    [Authorize(Roles = "PlataformAdministrator, Administrator")]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteEmployeeCommand, DatabaseOperationResponseViewModel> handler, 
        [FromQuery] DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteEmployee with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(command, cancellationToken);
        var status = Enum.Parse<EOperationStatus>(result.Status);

        return status switch
        {
            EOperationStatus.Successful => Accepted(Request.GetDisplayUrl(), result),

            EOperationStatus.Failed => BadRequest(result),

            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }
}