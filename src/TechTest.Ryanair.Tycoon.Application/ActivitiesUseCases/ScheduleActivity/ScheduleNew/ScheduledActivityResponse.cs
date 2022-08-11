namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.ScheduleNew;

public class ScheduledActivityResponse
{
    public Guid ActivityId { get; set; }

    public readonly static ScheduledActivityResponse Null = new() { ActivityId = Guid.Empty };
}
