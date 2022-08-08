using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Application.UseCase;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.ScheduleActivity;

public class ScheduleActivityCommand : ICommand
{
    public Guid[] AssignedWorkers { get; private set; }
    public TimedActivity Activity { get; private set; }

    public ScheduleActivityCommand(TimedActivity activity, params Guid[] assignedWorkers)
    {
        AssignedWorkers = assignedWorkers;
        Activity = activity;
    }

    public Result Validate()
    {
        return Array.TrueForAll(AssignedWorkers, workerId => workerId != Guid.Empty) && Activity is not null 
            ? Result.Success()
            : Result.Fail("INVALID_SCHEDULE_COMMAND", "Provided worker Id or activity is invalid");
    }
}
