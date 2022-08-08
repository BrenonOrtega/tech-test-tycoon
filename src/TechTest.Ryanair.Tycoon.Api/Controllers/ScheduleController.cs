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
    public async Task<IActionResult> Schedule(ScheduleActivityRequest request)
    {
        if (request is null)
            return BadRequest();

        var command = request.ToCommand();
    }
}