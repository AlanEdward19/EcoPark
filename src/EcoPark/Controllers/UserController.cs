namespace EcoPark.Controllers;

[Route("[controller]")]
[ApiController]
public class UserController(ILogger<UserController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetList([FromServices] IHandler<IEnumerable<Guid>?, IEnumerable<User>> handler,
        [FromBody] IEnumerable<Guid>? ids, CancellationToken cancellationToken)
    {
        return Ok(await handler.HandleAsync(ids, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromServices] IHandler<Guid, User> handler, Guid id, CancellationToken cancellationToken)
    {
        return Ok(await handler.HandleAsync(id, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertUserCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertUserCommand command, CancellationToken cancellationToken)
    {
        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateUserCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromServices] IHandler<Guid, DatabaseOperationResponseViewModel> handler, 
        Guid id, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken);

        if (result)
            return NoContent();

        return NotFound();
    }
}