using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TechTest.Ryanair.Tycoon.Api.Requests;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;

namespace TechTest.Ryanair.Tycoon.Api.Controllers;

[Route("/api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ILogger<ActivitiesController> _logger;
    private readonly IScheduleActivityUseCase _scheduler;
    private readonly ICreateActivityUseCase _createActivity;
    private readonly IGetActivityByIdUseCase _getById;

    public ActivitiesController(ILogger<ActivitiesController> logger, IScheduleActivityUseCase scheduler,
        ICreateActivityUseCase createActivity, IGetActivityByIdUseCase getById)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _createActivity = createActivity ?? throw new ArgumentNullException(nameof(createActivity));
        _getById = getById ?? throw new ArgumentNullException(nameof(getById));
    }

    [HttpPost("schedule")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Schedule([FromBody] ScheduleActivityRequest request)
    {
        if (request is null)
            return BadRequest();

        var createCommand = request.ToCommand();
        if (createCommand.IsFailed)
            return BadRequest(createCommand.Error);

        var result = await _scheduler.HandleAsync(createCommand.Value);

        if (result.IsFailed)
        {
            _logger.LogInformation("Failed scheduling activity of Type {type}, starting {startDate} - ending {endData} for workers {workers}.\nError: {error}.",
                request.Type, request.StartDate, request.FinishDate, string.Join(',', request.Workers ?? new()), JsonSerializer.Serialize(result.Error));

            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(Get), new { Id = result.Value.ActivityId }, result.Value);
    }

    [HttpPatch("schedule")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ScheduleExisting([FromBody]ScheduleActivityRequest request)
    {
        if (request is null)
            return BadRequest();

        var createCommand = request.ToCommand();
        if (createCommand.IsFailed)
            return BadRequest(createCommand.Error);

        var result = await _scheduler.HandleAsync(createCommand.Value);

        if (result.IsFailed)
        {
            _logger.LogInformation("Failed scheduling activity of Type {type}, starting {startDate} - ending {endData} for workers {workers}.\nError: {error}.",
                request.Type, request.StartDate, request.FinishDate, string.Join(',', request.Workers ?? new()), JsonSerializer.Serialize(result.Error));

            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(Get), new { Id = result.Value.ActivityId }, result.Value);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Post([FromBody]CreateActivityRequest request)
    {
        if (request is null)
            return BadRequest();

        var command = request.ToCommand();

        var result = await _createActivity.HandleAsync(command);

        if (result.IsFailed)
        {
            _logger.LogInformation("Failed creating activity of Type {type}, starting {startDate} - ending {endData}. Error: {error}",
                request.ActivityType, request.StartDate, request.FinishDate, JsonSerializer.Serialize(result.Error));

            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value);
    }

    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FoundActivityResponse))]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var request = new GetActivityByIdRequest() { Id = id };
        var command = request.ToCommand();
        var result = await _getById.HandleAsync(command);

        if (result.IsFailed && result.Error == ApplicationErrors.ActivityNotFound)
            return NotFound(result.Error);

        if (result.IsFailed)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}