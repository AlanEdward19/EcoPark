﻿using EcoPark.Application.ParkingSpaces.Delete;
using EcoPark.Application.ParkingSpaces.Get;
using EcoPark.Application.ParkingSpaces.List;
using EcoPark.Application.ParkingSpaces.Models;

namespace EcoPark.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class ParkingSpaceController(ILogger<ParkingSpaceController> logger) : ControllerBase
{
    [HttpPost("list")]
    public async Task<IActionResult> GetList([FromServices] IHandler<ListParkingSpacesQuery, IEnumerable<ParkingSpaceSimplifiedViewModel>> handler,
        [FromBody] ListParkingSpacesQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: ListParkingSpaces with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");
        
        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetById([FromServices] IHandler<GetParkingSpaceQuery, ParkingSpaceSimplifiedViewModel> handler,
        [FromQuery] GetParkingSpaceQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: GetParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(query))}");

        return Ok(await handler.HandleAsync(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromServices] IHandler<InsertParkingSpaceCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] InsertParkingSpaceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: InsertParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromServices] IHandler<UpdateParkingSpaceCommand, DatabaseOperationResponseViewModel> handler,
        [FromBody] UpdateParkingSpaceCommand command, Guid Id, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: UpdateParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        command.SetParkingSpaceId(Id);
        return Created(Request.GetDisplayUrl(), await handler.HandleAsync(command, cancellationToken));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromServices] IHandler<DeleteParkingSpaceCommand, DatabaseOperationResponseViewModel> handler,
        [FromQuery] DeleteParkingSpaceCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            $"Method Call: DeleteParkingSpace with parameters: \n{string.Join("\n", EntityPropertiesUtilities.GetEntityPropertiesAndValueAsIEnumerable(command))}");

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.Status == "Successful")
            return Accepted(Request.GetDisplayUrl(), result);

        if(result.Message == "No Parking space were found with this id")
            return NotFound(result);

        return BadRequest(result);
    }
}