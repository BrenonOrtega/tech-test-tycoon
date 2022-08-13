using Awarean.Sdk.Result;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TechTest.Ryanair.Tycoon.Api.Requests.Workers;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkersController : ControllerBase
{
    private readonly ILogger<WorkersController> _logger;
    private readonly ICreateWorkerUseCase _creator;
    private readonly IGetWorkerByIdUseCase _getById;
    private readonly IGetBusiestWorkersUseCase _getBusiest;

    public WorkersController(ILogger<WorkersController> logger, ICreateWorkerUseCase creator, IGetWorkerByIdUseCase getById, IGetBusiestWorkersUseCase useCase)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _creator = creator ?? throw new ArgumentNullException(nameof(creator));
        _getById = getById ?? throw new ArgumentNullException(nameof(getById));
        _getBusiest = useCase ?? throw new ArgumentNullException(nameof(useCase));
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.UnprocessableEntity)]
    public async Task<IActionResult> CreateWorker([FromBody]CreateWorkerRequest request)
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
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetWorker([FromRoute]Guid id)
    {
        var request = new GetWorkerByIdRequest() { Id = id };
        var result = await _getById.HandleAsync(request.ToCommand());

        if (result.IsFailed)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("busiest/weekly-top-ten")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetWeeklyBusiest()
    {
        var result = await _getBusiest.HandleAsync(new GetWeeklyTopTenBusiestWorkersCommand());

        if (result.IsFailed)
        {
            return NotFound(result.Error);
        }

        return  result.Value != Enumerable.Empty<Worker>() ? Ok(result.Value) : NotFound();
    }

    [HttpGet("busiest")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetBusiest([FromQuery]GetBusiestWorkersRequest request)
    {
        if (request is null)
            return BadRequest(ApplicationErrors.NullCommand);

        var result = await _getBusiest.HandleAsync(request.ToCommand());

        if (result.IsFailed)
        {
            return NotFound(result.Error);
        }

        return result.Value != Enumerable.Empty<Worker>() ? Ok(result.Value) : NotFound();
    }

}
