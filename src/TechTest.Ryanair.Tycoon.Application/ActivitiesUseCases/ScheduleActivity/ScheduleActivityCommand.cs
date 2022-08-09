using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;

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
        var validArray = AssignedWorkers is not null;
        var AllValidGuids = validArray && Array.TrueForAll(AssignedWorkers, workerId => workerId != Guid.Empty);
        var validActivity = Activity is not null && Activity != TimedActivity.Null;
            
        return validArray && AllValidGuids && validActivity
            ? Result.Success()
            : Result.Fail(ApplicationErrors.InvalidScheduleActivityCommand);
     }
}
