using TechTest.Ryanair.Tycoon.Application.ScheduleActivity;

namespace TechTest.Ryanair.Tycoon.Api.Requests
{
    public class ScheduleActivityRequest
    {
        public string Type { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime StartDate { get; set; }
        public List<string> Workers { get; set; }

        public ScheduleActivityCommand ToCommand() => new ScheduleActivityCommand()
    }
}