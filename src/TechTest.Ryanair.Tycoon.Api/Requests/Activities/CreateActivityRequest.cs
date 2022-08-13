using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;

namespace TechTest.Ryanair.Tycoon.Api.Requests.Activities
{
    public class CreateActivityRequest
    {
        public string ActivityType { get; init; }
        public Guid Id { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime FinishDate { get; init; }

        internal CreateActivityCommand ToCommand() => new(Id, ActivityType, StartDate, FinishDate);
    }
}
