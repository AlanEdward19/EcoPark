using EcoPark.Application.Punctuations.Get;
using EcoPark.Application.Punctuations.List;
using EcoPark.Application.Punctuations.Models;

namespace EcoPark.Presentation.Controllers;

/// <summary>
/// Endpoints para Operações relacionadas a Pontuações
/// </summary>
/// <param name="logger"></param>
[Route("[controller]")]
[Authorize(Roles = "Client")]
[ApiController]
public class PunctuationController(ILogger<PunctuationController> logger) : ControllerBase
{
    /// <summary>
    /// Método para listar todas as pontuações que o determinado usuario possui, podendo filtrar por uma lista de Ids
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Lista de Pontuações com suas devidas informações</returns>
    [Tags("Informações da Pontuação")]
    [ProducesResponseType(typeof(IEnumerable<PunctuationViewModel>), StatusCodes.Status200OK)]
    [HttpPost("list")]
    public async Task<IActionResult> GetList(
        [FromServices] IHandler<ListPunctuationsQuery, IEnumerable<PunctuationViewModel>> handler,
        [FromBody] ListPunctuationsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListPunctuations with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    /// <summary>
    /// Método para buscar uma pontuação pelo Id
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Pontuação com suas devidas informações</returns>
    [Tags("Informações da Pontuação")]
    [ProducesResponseType(typeof(PunctuationViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EntityNotFoundValueObject), StatusCodes.Status404NotFound)]
    [HttpGet]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetPunctuationQuery, PunctuationViewModel?> handler,
        [FromQuery] GetPunctuationQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetPunctuation with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        var requestUserInfo = EntityPropertiesUtilities.GetUserInfo(HttpContext.User);
        query.SetRequestUserInfo(requestUserInfo);

        var result = await handler.HandleAsync(query, cancellationToken);

        return result is not null ? Ok(result) : NotFound(new EntityNotFoundValueObject($"Punctuation not Found"));
    }
}