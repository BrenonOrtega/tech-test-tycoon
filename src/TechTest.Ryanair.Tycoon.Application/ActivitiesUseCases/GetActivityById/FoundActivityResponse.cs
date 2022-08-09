using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;

public class FoundActivityResponse
{
    public FoundActivityResponse(TimedActivity activity) => Activity = activity;

    public TimedActivity Activity { get; internal set; }

    public static readonly FoundActivityResponse Null = new(TimedActivity.Null);
}