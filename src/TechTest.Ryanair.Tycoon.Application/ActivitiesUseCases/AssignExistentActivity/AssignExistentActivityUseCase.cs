using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.AssignExistentActivity;

internal class AssignExistentActivityUseCase : IAssignExistentActivityUseCase
{
    private readonly ILogger<AssignExistentActivityUseCase> _logger;
    private readonly IBaseGetByIdUseCase _getById;
    private readonly IScheduleActivityUseCase _schedule;

    public AssignExistentActivityUseCase(ILogger<AssignExistentActivityUseCase> logger, IBaseGetByIdUseCase getById, IScheduleActivityUseCase schedule)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger)); 
        _getById = getById ?? throw new ArgumentNullException(nameof(getById));
        _schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
    }

    public async Task<Result<AssignedActivityResponse>> HandleAsync(AssignExistentActivityCommand command)
    {
        if (command is null)
            return Result.Fail<AssignedActivityResponse>(ApplicationErrors.NullCommand);

        var validation = command.Validate();
        if (validation.IsFailed)
            return Result.Fail<AssignedActivityResponse>(validation.Error);

        _logger.LogInformation("Getting activity id {id} to assign for workers {workers}", command.ActivityId, string.Join(',', command.Workers));

        var getCommand = new GetActivityByIdCommand(command.ActivityId);
        var getResult = await _getById.HandleAsync(getCommand);

        if (getResult.IsFailed)
            return Result.Fail<AssignedActivityResponse>(getResult.Error);

        var scheduleCommand = new ScheduleActivityCommand(getResult.Value, command.Workers.ToArray());
        var scheduleResult = await _schedule.HandleAsync(scheduleCommand);

        if (scheduleResult.IsFailed)
            return Result.Fail<AssignedActivityResponse>(getResult.Error);

        return Result.Success(new AssignedActivityResponse(scheduleResult.Value));
    }
}
