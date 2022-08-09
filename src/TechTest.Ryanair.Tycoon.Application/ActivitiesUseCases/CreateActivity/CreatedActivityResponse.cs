
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity
{
    public class CreatedActivityResponse
    {
        public CreatedActivityResponse(Guid id, DateTime startDate, DateTime finishDate, string restTime, string type)
        {
            Id = id;
            StartDate = startDate;
            FinishDate = finishDate;
            RestTime = restTime;
            Type = type;
        }

        public Guid Id { get; }
        public DateTime FinishDate { get; }
        public DateTime StartDate { get; }
        public string RestTime { get; }
        public string Type { get; }

        public static readonly CreatedActivityResponse Null = new(Guid.Empty, DateTime.MinValue, DateTime.MaxValue, "NO_TIME", "None");
    }
}