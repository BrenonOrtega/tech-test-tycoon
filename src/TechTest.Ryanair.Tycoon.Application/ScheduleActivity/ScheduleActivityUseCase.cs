using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ScheduleActivity;

public class ScheduleActivityUseCase : IScheduleActivityUseCase
{
    private readonly ILogger<ScheduleActivityUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ScheduleActivityUseCase(ILogger<ScheduleActivityUseCase> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<ScheduledActivityResponse>> HandleAsync(ScheduleActivityCommand command)
    {
        if (command is null || command.Validate().IsFailed)
            return Result.Fail<ScheduledActivityResponse>(ApplicationErrors.InvalidCommand);

        var workers = await _unitOfWork.WorkerRepository.GetWorkersAsync(command.AssignedWorkers);

        if (workers.Any() is false)
            return Result.Fail<ScheduledActivityResponse>(ApplicationErrors.WorkerNotFound);

        var tasks = new List<Task>();
        foreach (var worker in workers)
        {
            var result = worker.WorksIn(command.Activity);
            if(result.IsFailed)
            {
                _logger.LogInformation("Could not schedule activity {id} from {startDate} - {endDate}.\n Worker {id} {name} was actually {status}.",
                    command.Activity.Id, command.Activity.Start, command.Activity.Finish, worker.Id, worker.Name, worker.ActualStatus);

                return Result.Fail<ScheduledActivityResponse>(result.Error);
            }
            tasks.Add(_unitOfWork.WorkerRepository.UpdateAsync(worker.Id, worker));
        }

        tasks.Add(_unitOfWork.ActivityRepository.CreateAsync(command.Activity));
        tasks.Add(_unitOfWork.SaveAsync());

        await Task.WhenAll(tasks);

        return Result.Success(new ScheduledActivityResponse() { ActivityId = command.Activity.Id });
    }
}
