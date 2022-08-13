using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.ScheduleNew;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.AssignExistent;
using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.UpdateDates;
using TechTest.Ryanair.Tycoon.Api.Requests.Activities;

namespace TechTest.Ryanair.Tycoon.Api.Controllers;

[Route("/api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ILogger<ActivitiesController> _logger;
    private readonly IAssignExistentActivityUseCase _assignExistent;
    private readonly IScheduleNewActivityUseCase _scheduleNew;
    private readonly ICreateActivityUseCase _createActivity;
    private readonly IGetActivityByIdUseCase _getById;
    private readonly IUpdateActivityDatesUseCase _updateActivityDates;

    public ActivitiesController(ILogger<ActivitiesController> logger, IScheduleNewActivityUseCase scheduleNew,
        IAssignExistentActivityUseCase assignExistent, ICreateActivityUseCase createActivity, IGetActivityByIdUseCase getById,
        IUpdateActivityDatesUseCase updateActivityDates)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _assignExistent = assignExistent ?? throw new ArgumentNullException(nameof(assignExistent));
        _scheduleNew = scheduleNew ?? throw new ArgumentNullException(nameof(scheduleNew));
        _createActivity = createActivity ?? throw new ArgumentNullException(nameof(createActivity));
        _getById = getById ?? throw new ArgumentNullException(nameof(getById));
        _updateActivityDates = updateActivityDates ?? throw new ArgumentNullException(nameof(updateActivityDates));
    }

    [HttpPost("schedule")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Schedule([FromBody] ScheduleNewActivityRequest request)
    {
        if (request is null)
            return BadRequest();

        var createCommand = request.ToCommand();
        if (createCommand.IsFailed)
            return BadRequest(createCommand.Error);

        var result = await _scheduleNew.HandleAsync(createCommand.Value);

        if (result.IsFailed)
        {
            _logger.LogInformation("Failed scheduling activity of Type {type}, starting {startDate} - ending {endData} for workers {workers}.\nError: {error}.",
                request.Type, request.StartDate, request.FinishDate, string.Join(',', request.Workers ?? new()), JsonSerializer.Serialize(result.Error));

            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(Get), new { Id = result.Value.ActivityId }, result.Value);
    }

    [HttpPatch("schedule")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> ScheduleExisting([FromBody]AssignExistentActivityRequest request)
    {
        if (request is null)
            return BadRequest();

        var command = request.ToCommand();
        var result = await _assignExistent.HandleAsync(command);

        if (result.IsFailed)
        {
            _logger.LogInformation("Failed scheduling activity {id} for workers {workers}\nError: {error}.",
                request.ActivityId, string.Join(',', request.WorkerIds ?? new()), JsonSerializer.Serialize(result.Error));

            if (result.Error == ApplicationErrors.ActivityNotFound)
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return Accepted(result.Value);
    }

    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Post([FromBody]CreateActivityRequest request)
    {
        if (request is null)
            return BadRequest(Error.Create("INVALID_REQUEST", "Received an invalid request when posting an activity."));

        var command = request.ToCommand();

        var result = await _createActivity.HandleAsync(command);

        if (result.IsFailed)
        {
            _logger.LogInformation("Failed creating activity of Type {type}, starting {startDate} - ending {endDate}. Error: {error}",
                request.ActivityType, request.StartDate, request.FinishDate, JsonSerializer.Serialize(result.Error));

            return BadRequest(result.Error);
        }

        return CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value);
    }

    [HttpPatch]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> Patch([FromBody] UpdateActivityDatesRequest request)
    {
        if (request is null)
            return BadRequest(Error.Create("INVALID_REQUEST", "Received an invalid request when posting an activity."));

        var command = request.ToCommand();

        var result = await _updateActivityDates.HandleAsync(command);

        if (result.IsFailed)
        {
            _logger.LogInformation("Failed updating activity {activityId} dates for starting date {startDate} - ending date {endDate}. Error: {error}",
                request.Id, request.NewStartDate, request.NewFinishDate, JsonSerializer.Serialize(result.Error));

            return BadRequest(result.Error);
        }

        return Ok(result.Value);
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