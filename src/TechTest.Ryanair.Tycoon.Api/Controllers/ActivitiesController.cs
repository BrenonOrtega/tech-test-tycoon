using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TechTest.Ryanair.Tycoon.Api.Requests;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;

namespace TechTest.Ryanair.Tycoon.Api.Controllers;

[Route("/api/[controller]")]
public class ActivitiesController : ControllerBase
{
    private readonly ILogger<ActivitiesController> _logger;
    private readonly IScheduleActivityUseCase _scheduler;
    private readonly ICreateActivityUseCase _createActivity;

    public ActivitiesController(ILogger<ActivitiesController> logger, IScheduleActivityUseCase scheduler, ICreateActivityUseCase createActivity)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _createActivity = createActivity ?? throw new ArgumentNullException(nameof(createActivity));
    }

    [HttpPost("schedule")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Schedule([FromBody]ScheduleActivityRequest request)
    {
        if (request is null)
            return BadRequest();

        var command = request.ToCommand();

        var result = await _scheduler.HandleAsync(command);

        if(result.IsFailed)
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
    public async Task<IActionResult> Post([FromBody] CreateActivityRequest request)
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
    public async Task<IActionResult> Get([FromRoute]GetActivityByIdRequest request) => throw new NotImplementedException();


}