using System.Collections.Immutable;
using TechTest.Ryanair.Tycoon.Domain.FluentApi.WorkerFluent;

namespace TechTest.Ryanair.Tycoon.Domain.Entities;

public class Worker : IActivityWorker
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    private readonly HashSet<TimedActivity> _activities = new();
    public ImmutableHashSet<TimedActivity> Activities => _activities.ToImmutableHashSet(); 
    public Status ActualStatus
    {
        get
        {
            var moment = DateTime.Now;
            if (_activities.Any(x => x.Start < moment && x.Finish > moment))
                return Status.Working;

            if (_activities.Any(x => x.Finish < moment && x.FinishRestingDate > DateTime.Now))
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
        if (this == Worker.Null)
            return new WorksInResult(DomainErrors.AddingActivityToNullWorker, activity);

        if (_activities.Contains(activity))
            return new WorksInResult(DomainErrors.AlreadyWorksInActivity, activity);

        if(_activities.Any(act => act.Overlaps(activity)))
            return new WorksInResult(DomainErrors.OverlappingActivities, activity);

        _activities.Add(activity);
        var result = activity.HaveParticipant(this);
        
        if(result.IsFailed)
            return new WorksInResult(result.Error, activity);
            
        return new WorksInResult(this);

    }

    public enum Status
    {
        Working,
        Recharging,
        Idle
    }

    public static readonly Worker Null = new Worker(Guid.Empty, string.Empty);
}