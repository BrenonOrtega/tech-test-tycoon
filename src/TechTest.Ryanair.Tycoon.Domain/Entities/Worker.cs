using TechTest.Ryanair.Tycoon.Domain.FluentApi.Activity;

namespace TechTest.Ryanair.Tycoon.Domain.Entities;

public class Worker : IActivityWorker
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public WorkingStatus Status
    {
        get
        {
            if (Activities.Any(x => x.Start < DateTime.Now && x.Finish > DateTime.Now))
                return WorkingStatus.Working;

            if (Activities.Any(x => x.Start < DateTime.Now && x.Finish > DateTime.Now && x.Finish + x.RestTime < DateTime.Now))
                return WorkingStatus.Recharging;

            return WorkingStatus.Idle;
        }
    }
    public List<TimedActivity> Activities { get; } = new();

    public Worker(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public WorksInResult WorksIn(TimedActivity activity)
    {
        if (Activities.Exists(act => act.Equals(activity)))
            return new WorksInResult(DomainErrors.AlreadyWorksInActivity)
    }

    public enum WorkingStatus
    {
        Working,
        Recharging,
        Idle
    }
}