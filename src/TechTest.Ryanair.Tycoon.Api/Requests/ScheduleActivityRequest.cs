using System.ComponentModel.DataAnnotations;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Api.Requests
{
    public class ScheduleActivityRequest
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public DateTime FinishDate { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public List<string> Workers { get; set; }
        public Guid? Id { get; set; }

        public ScheduleActivityCommand ToCommand() => new ScheduleActivityCommand(CreateActivity(), UseIds());

        private Guid[] UseIds()
        {
            if (Workers is null)
                return Array.Empty<Guid>();

            return Workers.Select(x => Guid.TryParse(x, out var guid) ? guid : Guid.Empty).ToArray();
        }

        // Code Smell, Should Remove it to use case or an Service for API to construct components.
        private TimedActivity CreateActivity()
        {
            var id = Id ?? Guid.NewGuid();
            if (Type == "Component")
                return new BuildComponentActivity(id, StartDate, FinishDate);

            if (Type == "Machine")
                return new BuildComponentActivity(id, StartDate, FinishDate);

            return TimedActivity.Null;
        }
    }
}