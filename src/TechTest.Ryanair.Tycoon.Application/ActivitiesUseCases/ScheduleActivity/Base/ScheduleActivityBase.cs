using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.ScheduleNew;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.Base;

internal class ScheduleActivityBase : IScheduleActivityBase
{
    private readonly ILogger<ScheduleActivityBase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleActivityBase(ILogger<ScheduleActivityBase> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<TimedActivity>> HandleAsync(ScheduleActivityCommand command)
    {
        if (command is null)
            return Result.Fail<TimedActivity>(ApplicationErrors.InvalidCommand);

        var validation = command.Validate();
        if (validation.IsFailed)
            return Result.Fail<TimedActivity>(validation.Error);

        var workers = await _unitOfWork.WorkerRepository.GetWorkersAsync(command.AssignedWorkers);
        if (workers.Any() is false)
            return Result.Fail<TimedActivity>(ApplicationErrors.WorkerNotFound);

        var scheduleResult = await ScheduleForWorkers(command.Activity, workers);

        if (scheduleResult.IsFailed)
        {
            await RollbackWorkersAsync(command.Activity, workers);

            return Result.Fail<TimedActivity>(scheduleResult.Error);
        }

        return Result.Success(command.Activity);
    }

    private async Task<Result> ScheduleForWorkers(TimedActivity activity, IEnumerable<Worker> workers)
    {
        var tasks = new List<Task>();
        var results = new List<Result>();

        foreach (var worker in workers)
        {
            var result = worker.WorksIn(activity);

            if (result.IsFailed)
            {
                _logger.LogInformation("Could not schedule activity {id} from {startDate} - {endDate}.\n Worker {id} {name} was actually {status}.",
                    activity.Id, activity.Start, activity.Finish, worker.Id, worker.Name, worker.ActualStatus);

                return Result.Fail<ScheduledActivityResponse>(result.Error);
            }
            var updateWorker = async () => results.Add(await _unitOfWork.WorkerRepository.UpdateAsync(worker.Id, worker));
            tasks.Add(updateWorker.Invoke());
        }

        return await UpdateAndCheckIntegrity(tasks, results);
    }

    private static async Task<Result> UpdateAndCheckIntegrity(List<Task> tasks, List<Result> results)
    {
        await Task.WhenAll(tasks);

        var failed = results.FirstOrDefault(x => x.IsFailed);

        if (failed != default)
            return Result.Fail(failed.Error);

        return Result.Success();
    }

    private async Task<Result> RollbackWorkersAsync(TimedActivity activity, IEnumerable<Worker> workers)
    {
        foreach (var worker in workers)
        {
            var result = worker.Unassign(activity);

            if (result.IsFailed)
            {
                _logger.LogError("Could not rollback assignment activity {id} from {startDate} - {endDate}.\n Worker {id} {name}.",
                    activity.Id, activity.Start, activity.Finish, worker.Id, worker.Name);

                return Result.Fail(result.Error);
            }
        }

        return Result.Success();
    }
}
