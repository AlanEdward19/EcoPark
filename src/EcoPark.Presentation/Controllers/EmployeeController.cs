using EcoPark.Application.Employees.Delete;
using EcoPark.Application.Employees.Get;
using EcoPark.Application.Employees.Insert;
using EcoPark.Application.Employees.List;
using EcoPark.Application.Employees.Models;
using EcoPark.Application.Employees.Update;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployeeController(ILogger<EmployeeController> logger) : ControllerBase
{
    [HttpPut("login")]
    public async Task<IActionResult> Login([FromServices] IHandler<LoginQuery, LoginViewModel> handler, [FromBody] LoginQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: Login [Employees] with email: {query.Email}");

        query.SetIsEmployee(true);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result == null)
            return NotFound(new{Message = "User not found" });

        return Ok(result);
    }

    [HttpPost("list")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListEmployeesQuery, IEnumerable<EmployeeViewModel>> handler,
        [FromBody] ListEmployeesQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListEmployees with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetEmployeeQuery, EmployeeViewModel> handler, 
        [FromQuery] GetEmployeeQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetEmployee with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertEmployeeCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertEmployeeCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertEmployee with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPatch]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateEmployeeCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] UpdateEmployeeCommand command, Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateEmployee with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        command.SetEmployeeId(id);
        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpDelete]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteEmployeeCommand, DatabaseOperationResponseViewModel> handler, 
        [FromQuery] DeleteEmployeeCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteEmployee with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.Status == "Successful")
            return Accepted(Request.GetDisplayUrl(), result);

        if (result.Message == "No Employees were found with this id")
            return NotFound(result);

        return BadRequest(result);
    }
}