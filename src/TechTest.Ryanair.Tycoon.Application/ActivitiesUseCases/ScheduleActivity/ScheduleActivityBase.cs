using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;

internal class ScheduleActivityBase : IScheduleActivityUseCase
{
    private readonly ILogger<ScheduleActivityBase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleActivityBase(ILogger<ScheduleActivityBase> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<ScheduledActivityResponse>> HandleAsync(ScheduleActivityCommand command)
    {
        if (command is null)
            return Result.Fail<ScheduledActivityResponse>(ApplicationErrors.InvalidCommand);

        var validation = command.Validate();
        if (validation.IsFailed)
            return Result.Fail<ScheduledActivityResponse>(validation.Error);

        var workers = await _unitOfWork.WorkerRepository.GetWorkersAsync(command.AssignedWorkers);
        if (workers.Any() is false)
            return Result.Fail<ScheduledActivityResponse>(ApplicationErrors.WorkerNotFound);

        return await ScheduleForWorkers(command.Activity, workers);
    }

    private async Task<Result<ScheduledActivityResponse>> ScheduleForWorkers(TimedActivity activity, IEnumerable<Worker> workers)
    {
        foreach (var worker in workers)
        {
            var result = worker.WorksIn(activity);

            if (result.IsFailed)
            {
                _logger.LogInformation("Could not schedule activity {id} from {startDate} - {endDate}.\n Worker {id} {name} was actually {status}.",
                    activity.Id, activity.Start, activity.Finish, worker.Id, worker.Name, worker.ActualStatus);

                return Result.Fail<ScheduledActivityResponse>(result.Error);
            }
        }

        return Result.Success(new ScheduledActivityResponse() { ActivityId = activity.Id });
    }

    private async Task<Result> RollbackWorkersAsync(IEnumerable<Worker> workers, TimedActivity activity)
    {
        var tasks = new List<Task>(workers.Count());
        foreach (var worker in workers)
        {
            var result = worker.Unassign(activity);

            if (result.IsFailed)
            {
                _logger.LogError("Could not rollback assignment activity {id} from {startDate} - {endDate}.\n Worker {id} {name}.",
                    activity.Id, activity.Start, activity.Finish, worker.Id, worker.Name);

                return Result.Fail(result.Error);
            }

            tasks.Add(_unitOfWork.WorkerRepository.UpdateAsync(worker.Id, worker));
        }

        await Task.WhenAll(tasks);
        var updated = await _unitOfWork.SaveAsync();
        return updated;
    }
}
