namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.UpdateDates;

public class UpdatedActivityDatesResponse
{
    public Guid Id { get; }
    public DateTime StartDate { get; }
    public DateTime FinishDate { get; }

    public UpdatedActivityDatesResponse(Guid id, DateTime startDate, DateTime finishDate) 
        => (Id, StartDate, FinishDate) = (id, startDate, finishDate);

    public static readonly UpdatedActivityDatesResponse Null = new(Guid.Empty, DateTime.MinValue, DateTime.MaxValue);
}
