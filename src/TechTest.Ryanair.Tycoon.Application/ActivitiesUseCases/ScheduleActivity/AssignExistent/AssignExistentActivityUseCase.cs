using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById.Base;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.Base;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.ScheduleNew;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.AssignExistent;

internal class AssignExistentActivityUseCase : IAssignExistentActivityUseCase
{
    private readonly ILogger<AssignExistentActivityUseCase> _logger;
    private readonly IBaseGetByIdUseCase _getById;
    private readonly IScheduleActivityBase _schedule;
    private readonly IUnitOfWork _unitOfWork;

    public AssignExistentActivityUseCase(ILogger<AssignExistentActivityUseCase> logger, IBaseGetByIdUseCase getById, IScheduleActivityBase schedule, IUnitOfWork unitOfWork)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _getById = getById ?? throw new ArgumentNullException(nameof(getById));
        _schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<AssignedActivityResponse>> HandleAsync(AssignExistentActivityCommand command)
    {
        if (command is null)
            return Result.Fail<AssignedActivityResponse>(ApplicationErrors.NullCommand);

        var validation = command.Validate();
        if (validation.IsFailed)
            return Result.Fail<AssignedActivityResponse>(validation.Error);

        _logger.LogInformation("Getting activity id {id} to assign for workers {workers}", command.ActivityId, string.Join(',', command.Workers));

        var getActivityCommand = new GetActivityByIdCommand(command.ActivityId);
        var getActivityResult = await _getById.HandleAsync(getActivityCommand);

        if (getActivityResult.IsFailed)
            return Result.Fail<AssignedActivityResponse>(getActivityResult.Error);

        var activity = getActivityResult.Value;
        var scheduleCommand = new ScheduleActivityCommand(activity, command.Workers.ToArray());
        var scheduleResult = await _schedule.HandleAsync(scheduleCommand);

        if (scheduleResult.IsFailed)
            return Result.Fail<AssignedActivityResponse>(scheduleResult.Error);

        await _unitOfWork.ActivityRepository.UpdateAsync(activity.Id, activity);

        return Result.Success(new AssignedActivityResponse(activity.Id));
    }
}
