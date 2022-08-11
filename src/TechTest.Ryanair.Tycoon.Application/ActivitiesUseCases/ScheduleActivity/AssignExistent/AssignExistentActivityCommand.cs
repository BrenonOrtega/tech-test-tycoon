using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.AssignExistent;

public class AssignExistentActivityCommand : ICommand
{
    public Guid ActivityId { get; }
    public IEnumerable<Guid> Workers { get; }

    public AssignExistentActivityCommand(Guid activityId, IEnumerable<Guid> workers)
    {
        ActivityId = activityId;
        Workers = workers;
    }

    public Result Validate()
    {
        if (ActivityId == Guid.Empty)
            return Result.Fail(ApplicationErrors.InvalidGuid);

        if (Workers.Any() is false || Workers.Any(x => x == Guid.Empty))
            return Result.Fail(ApplicationErrors.InvalidCommand);

        return Result.Success();
    }
}