using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.ScheduleNew;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;

public class ScheduleNewActivityUseCase : IScheduleNewActivityUseCase
{
    private readonly ILogger<ScheduleNewActivityUseCase> _logger;
    private readonly IScheduleActivityBase _scheduleBase;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleNewActivityUseCase(ILogger<ScheduleNewActivityUseCase> logger, IScheduleActivityBase scheduleBase, IUnitOfWork unitOfWork)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _scheduleBase = scheduleBase ?? throw new ArgumentNullException(nameof(scheduleBase));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<ScheduledActivityResponse>> HandleAsync(ScheduleActivityCommand command)
    {
        var result = await _scheduleBase.HandleAsync(command);

        if (result.IsFailed)
        {
            _logger.LogCritical("Cannot continue creating activity  workflow when updating workers not completed succesfully");
            return Result.Fail<ScheduledActivityResponse>(result.Error);
        }

        await _unitOfWork.ActivityRepository.CreateAsync(result.Value);
        await _unitOfWork.SaveAsync();

        return Result.Success(new ScheduledActivityResponse() { ActivityId = result.Value.Id });
    }
}
