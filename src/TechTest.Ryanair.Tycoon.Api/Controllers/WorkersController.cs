using Awarean.Sdk.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechTest.Ryanair.Tycoon.Api.Requests;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.CreateWorker;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkersController : ControllerBase
{
    private readonly ILogger<WorkersController> _logger;
    private readonly ICreateWorkerUseCase _creator;

    public WorkersController(ILogger<WorkersController> logger, ICreateWorkerUseCase creator)
    {
       _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _creator = creator ?? throw new ArgumentNullException(nameof(creator));
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorker(CreateWorkerRequest request)
    {
        if (request is null)
            return BadRequest(Error.Create("INVALID_REQUEST", "One or more validation errors ocurred for the request."));

        var command = request.ToCommand();

        var result = await _creator.HandleAsync(command);

        if (result.IsFailed && result.Error == ApplicationErrors.InvalidCommand)
            return UnprocessableEntity(Error.Create("ERROR_PROCESSING_COMMAND", "One or more validation errors ocurred for the command."));

        return CreatedAtAction(nameof(GetWorker), new {id = result.Value.Id }, result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetWorker(Guid id)
    {
        /*var result = await _getById.HandleAsync(id);

        if (result.IsSuccess && result.Value == Worker.Null)
            return NotFound(result.Error);

        return Ok(result.Value);*/

        return Problem("This endpoint is not implemented yet.");
    }

}
