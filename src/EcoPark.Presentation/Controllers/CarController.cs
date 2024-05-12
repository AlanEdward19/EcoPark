using EcoPark.Application.Cars.Delete;
using EcoPark.Application.Cars.Get;
using EcoPark.Application.Cars.Insert;
using EcoPark.Application.Cars.List;
using EcoPark.Application.Cars.Models;
using EcoPark.Application.Cars.Update;

namespace EcoPark.Presentation.Controllers;

/// <summary>
/// Endpoints para Operações relacionadas a Carros
/// </summary>
/// <param name="logger"></param>
[Route("[controller]")]
[ApiController]
[Authorize(Roles = "Client")]
public class CarController(ILogger<CarController> logger) : ControllerBase
{
    /// <summary>
    /// Método para buscar um carro pelo Id
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Carro com suas devidas informações</returns>
    [Tags("Informações do Carro")]
    [ProducesResponseType(typeof(CarViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EntityNotFoundValueObject), StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetCarQuery, CarViewModel?> handler,
        [FromQuery] GetCarQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetCar with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(query, cancellationToken);

        return result is not null ? Ok(result) : NotFound(new EntityNotFoundValueObject($"Car not Found"));
    }

    /// <summary>
    /// Método para listar todos os carros que o determinado usuario possui, podendo filtrar por uma lista de Ids
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de Carros com suas devidas informações</returns>
    [Tags("Informações do Carro")]
    [ProducesResponseType(typeof(IEnumerable<CarViewModel>), StatusCodes.Status200OK)]
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

    /// <summary>
    /// Método para inserir um novo carro atrelado a um usuario em especifico que está logado.
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Carro")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
    [HttpPost]
    public async Task<IActionResult> Insert(
        [FromServices] IHandler<InsertCarCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertCarCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertCar with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

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
    /// Método para atualizar um carro atrelado a um usuario em especifico que está logado.
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Carro")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
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

            EOperationStatus.NotFound => NotFound(result),

            EOperationStatus.Failed => BadRequest(result),

            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }

    /// <summary>
    /// Método para deletar um carro atrelado a um usuario em especifico que está logado.
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Mensagem sobre resultado da operação</returns>
    [Tags("Operações do Carro")]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(DatabaseOperationResponseViewModel), StatusCodes.Status401Unauthorized)]
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

            EOperationStatus.NotFound => NotFound(result),

            EOperationStatus.Failed => BadRequest(result),

            EOperationStatus.NotAuthorized => Unauthorized(result)
        };
    }
}
