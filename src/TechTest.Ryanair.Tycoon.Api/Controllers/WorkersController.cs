using Awarean.Sdk.Result;
using Microsoft.AspNetCore.Mvc;
using TechTest.Ryanair.Tycoon.Api.Requests;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;

namespace TechTest.Ryanair.Tycoon.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkersController : ControllerBase
{
    private readonly ILogger<WorkersController> _logger;
    private readonly ICreateWorkerUseCase _creator;
    private readonly IGetWorkerByIdUseCase _getById;

    public WorkersController(ILogger<WorkersController> logger, ICreateWorkerUseCase creator, IGetWorkerByIdUseCase getById)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _creator = creator ?? throw new ArgumentNullException(nameof(creator));
        _getById = getById ?? throw new ArgumentNullException(nameof(getById));
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

        return CreatedAtAction(nameof(GetWorker), new { id = result.Value.Id }, result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetWorker(GetWorkerByIdRequest request)
    {
        var result = await _getById.HandleAsync(request.ToCommand());

        if (result.IsFailed)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

}
