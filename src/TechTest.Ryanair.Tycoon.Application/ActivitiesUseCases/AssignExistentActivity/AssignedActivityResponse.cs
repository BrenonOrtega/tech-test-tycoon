using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.AssignExistentActivity;

internal class AssignedActivityResponse
{
    public AssignedActivityResponse(ScheduledActivityResponse value)
    {
        Id = value.ActivityId;
    }

    private AssignedActivityResponse(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }

    public static readonly AssignedActivityResponse Null = new(Guid.Empty);
}