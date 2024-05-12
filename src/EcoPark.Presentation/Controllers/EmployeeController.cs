using EcoPark.Application.Employees.Delete;
using EcoPark.Application.Employees.Delete.GroupAccess;
using EcoPark.Application.Employees.Get;
using EcoPark.Application.Employees.Insert;
using EcoPark.Application.Employees.Insert.GroupAccess;
using EcoPark.Application.Employees.Insert.System;
using EcoPark.Application.Employees.List;
using EcoPark.Application.Employees.Models;
using EcoPark.Application.Employees.Update;

namespace EcoPark.Presentation.Controllers;

/// <summary>
/// Endpoints para Operações relacionadas a Funcionários
/// </summary>
/// <param name="logger"></param>
[Route("[controller]")]
[ApiController]
public class EmployeeController(ILogger<EmployeeController> logger) : ControllerBase
{
    /// <summary>
    /// Método para listar todos os funcionários cadastrados atrelados a um administrador
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de funcionarios</returns>
    [Tags("Informações do Funcionário")]
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

    /// <summary>
    /// Método para buscar um funcionário pelo Id
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Funcionario</returns>
    [Tags("Informações do Funcionário")]
    [ProducesResponseType(typeof(EmployeeViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EntityNotFoundValueObject), StatusCodes.Status404NotFound)]
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

        return result is not null ? Ok(result) : NotFound(new EntityNotFoundValueObject($"Employee not found"));
    }

    /// <summary>
    /// Método para inserir um novo funcionário
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Funcionário")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Authorize(Roles = "PlataformAdministrator, Administrator")]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertEmployeeCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] InsertEmployeeCommand command, [FromForm] IFormFile image ,CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertEmployee with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        command.SetRequestUserInfo(requestUserInfo);
        command.SetImage(image, image.FileName, cancellationToken);

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
    /// Método para inserir um novo funcionário
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Funcionário")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPost("system")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> InsertSystem([FromServices] IHandler<InsertSystemCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertSystemCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertSystem with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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
    /// Método para adicionar permissão de acesso a uma localização para um funcionario
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Funcionário")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPost("GroupAccess")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> InsertGroupAccess([FromServices] IHandler<InsertEmployeeGroupAccessCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] InsertEmployeeGroupAccessCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertGroupAccess with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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
    /// Método para deletar permissão de acesso a uma localização para um funcionario
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Funcionário")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpDelete("GroupAccess")]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteGroupAccess([FromServices] IHandler<DeleteEmployeeGroupAccessCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteEmployeeGroupAccessCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteGroupAccess with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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
    /// Método para atualizar um funcionário
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Funcionário")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
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

            EOperationStatus.NotFound => NotFound(result),

            EOperationStatus.Failed => BadRequest(result),

            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para deletar um funcionário
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Funcionário")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
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

            EOperationStatus.NotFound => NotFound(result),

            EOperationStatus.Failed => BadRequest(result),

            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }
}