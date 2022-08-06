using TechTest.Ryanair.Tycoon.Domain.FluentApi.Worker;

namespace TechTest.Ryanair.Tycoon.Domain.Entities;

public class Worker : IActivityWorker
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<TimedActivity> Activities { get; } = new();
    public Status ActualStatus
    {
        get
        {
            var moment = DateTime.Now;
            if (Activities.Any(x => x.Start < moment && x.Finish > moment))
                return Status.Working;

            if (Activities.Any(x => x.Finish < moment && x.FinishRestingDate > DateTime.Now))
                return Status.Recharging;

            return Status.Idle;
        }
    }

    public Worker(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public WorksInResult WorksIn(TimedActivity activity)
    {
        if (Activities.Exists(act => act.Equals(activity)))
            return new WorksInResult(DomainErrors.AlreadyWorksInActivity, activity);

        if(Activities.Any(act => act.Overlaps(activity)))
            return new WorksInResult(DomainErrors.OverlappingActivities, activity);

        return new WorksInResult(this);
    }

    public enum Status
    {
        Working,
        Recharging,
        Idle
    }
}