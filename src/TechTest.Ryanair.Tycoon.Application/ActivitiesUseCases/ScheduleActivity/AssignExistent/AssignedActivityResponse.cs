namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.AssignExistent;

public class AssignedActivityResponse
{
    public AssignedActivityResponse(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }

    public static readonly AssignedActivityResponse Null = new(Guid.Empty);
}