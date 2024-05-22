namespace EcoPark.Presentation.Controllers;

/// <summary>
/// Endpoints para Operações relacionadas a Login
/// </summary>
/// <param name="logger"></param>
[Route("[controller]")]
[ApiController]
public class LoginController(ILogger<LoginController> logger) : ControllerBase
{
    /// <summary>
    /// Método para realizar o login de um usuario
    /// </summary>
    /// <param name="handler"></param>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Email do usuário e seu token</returns>
    [Tags("Login")]
    [ProducesResponseType(typeof(LoginViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(EntityNotFoundValueObject), StatusCodes.Status404NotFound)]
    [HttpPut]
    public async Task<IActionResult> Login([FromServices] IHandler<LoginQuery, LoginViewModel> handler, [FromBody] LoginQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: Login with email: {query.Email}");

        var result = await handler.HandleAsync(query, cancellationToken);

        return result is not null ? Ok(result) : NotFound(new EntityNotFoundValueObject($"User not found"));
    }
}