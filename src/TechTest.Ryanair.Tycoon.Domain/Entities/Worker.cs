using TechTest.Ryanair.Tycoon.Domain.FluentApi.Activity;

namespace TechTest.Ryanair.Tycoon.Domain.Entities;

public class Worker : IActivityWorker
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Activity> Activities { get; set; }
    public WorkerStatus Status { get; set; }

    public WorksInResult WorksIn(Activity activity)
    {
        Activities.Any(x => x.StartDate <= activity )
    }
}
    public enum WorkerStatus
    {
        Recharging,
        Working,
        Idle
    }