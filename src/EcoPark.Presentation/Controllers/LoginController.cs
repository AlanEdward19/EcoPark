namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class LoginController(ILogger<LoginController> logger) : ControllerBase
{
    [HttpPut]
    public async Task<IActionResult> Login([FromServices] IHandler<LoginQuery, LoginViewModel> handler, [FromBody] LoginQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: Login with email: {query.Email}");

        var result = await handler.HandleAsync(query, cancellationToken);

        if (result == null)
            return NotFound(new { Message = "User not found" });

        return Ok(result);
    }
}