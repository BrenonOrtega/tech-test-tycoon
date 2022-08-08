using Microsoft.AspNetCore.Mvc;
using TechTest.Ryanair.Tycoon.Api.Requests;
using TechTest.Ryanair.Tycoon.Application.ScheduleActivity;

namespace TechTest.Ryanair.Tycoon.Api.Controllers;

[Route("/api/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly ILogger<ScheduleController> _logger;
    private readonly IScheduleActivityUseCase _scheduler;

    public ScheduleController(ILogger<ScheduleController> logger, IScheduleActivityUseCase scheduler)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
    }

    [HttpPost]
    public async Task<IActionResult> Schedule([FromBody]ScheduleActivityRequest request)
    {
        if (request is null)
            return BadRequest();

        var command = request.ToCommand();

        var result = await _scheduler.HandleAsync(command);

        if(result.IsFailed)
        {
            _logger.LogInformation("Failed scheduling activity of Type {type}, starting {startDate} - ending {endData} for workers {workers}",
                request.Type, request.StartDate, request.FinishDate, string.Join(',', request.Workers ?? new()));

            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}