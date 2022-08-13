using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.UpdateDates;

namespace TechTest.Ryanair.Tycoon.Api.Requests.Activities;

public class UpdateActivityDatesRequest
{
    public Guid Id { get; set; }
    public DateTime NewFinishDate { get; set; }
    public DateTime NewStartDate { get; set; }

    internal UpdateActivityDatesCommand ToCommand() => new(Id, NewStartDate, NewFinishDate);
}
