namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.UpdateDates;

public class UpdatedActivityDatesResponse
{
    public Guid Id { get; }
    public UpdatedActivityDatesResponse(Guid id) => Id = id;
    public static readonly UpdatedActivityDatesResponse Null = new(Guid.Empty);
}
