using Awarean.Sdk.Result;
using System.Collections.Immutable;
using TechTest.Ryanair.Tycoon.Domain.FluentApi.WorkerFluent;

namespace TechTest.Ryanair.Tycoon.Domain.Entities;

public class Worker : IActivityWorker
{
    private readonly Dictionary<Guid, TimedActivity> _activities = new();

    public Worker(Guid id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public ImmutableHashSet<TimedActivity> Activities => _activities.Values.ToImmutableHashSet();
    public Status ActualStatus => GetStatus();

    public WorksInResult WorksIn(TimedActivity activity)
    {
        if (this == Null)
            return new WorksInResult(DomainErrors.AddingActivityToNullWorker, activity);

        if (activity is null || activity == TimedActivity.Null)
            return new WorksInResult(DomainErrors.TryWorkingInInvalidActivity, TimedActivity.Null);

        if (Activities.Contains(activity))
            return new WorksInResult(DomainErrors.AlreadyWorksInActivity, activity);

        if (Activities.Any(act => act.Overlaps(activity)))
            return new WorksInResult(DomainErrors.OverlappingActivities, activity);

        if(Activities.Any(act => act.OverlapsByRest(activity)))
            return new WorksInResult(DomainErrors.ActivityScheduledInRestTime, activity);

        _activities.Add(activity.Id, activity);
        var result = activity.HaveParticipant(this);

        if (result.IsFailed)
            return new WorksInResult(result.Error, activity);

        return new WorksInResult(this);
    }

    public Result Unassign(TimedActivity activity)
    {
        if(_activities.Remove(activity.Id, out _))
        {
            activity.WorksNoMore(this);
            return Result.Success();
        }

        return Result.Fail(DomainErrors.ActivityNotAssignedToWorker);
    }

    public bool CannotWorkNewShift(Guid activityId, DateTime startDate, DateTime endDate)
    {
        var cannotAttend = _activities.Where(x => x.Key != activityId).Any(x => x.Value.WouldOverLapRest(startDate, endDate));

        return cannotAttend;
    }

     public TimeSpan WorkTimeBetween(DateTime initialDate, DateTime finalDate)
    {
        var overlaps = Activities.Where(x => x.WouldOverLap(initialDate, finalDate));

        var workTime = overlaps.Select(activity => EndPeriod(finalDate, activity) - StartPeriod(initialDate, activity))
            .Aggregate(TimeSpan.Zero, (initial, next) => initial + next);

        return workTime;

        static DateTime StartPeriod(DateTime initialDate, TimedActivity activity) 
            => activity.Start < initialDate ? initialDate: activity.Start;

        static DateTime EndPeriod(DateTime finalDate, TimedActivity activity) 
            => activity.Finish > finalDate ? finalDate : activity.Finish;
    }

    internal void GetNewShift(TimedActivity activity)
    {
        var unassignment = Unassign(_activities[activity.Id]);

        if (unassignment.IsFailed)
            throw new InvalidOperationException($"Exception happened when updating schedule {activity.Id} for worker {Id}.");

        var worksInResult = WorksIn(activity);

        if (worksInResult.IsFailed)
            throw new InvalidOperationException($"Exception happened when updating schedule {activity.Id} for worker {Id}.");
    }

    private Status GetStatus()
    {
        var moment = DateTime.Now;
        if (Activities.Any(x => x.Start < moment && x.Finish > moment))
            return Status.Working;

        if (Activities.Any(x => x.Finish < moment && x.FinishRestingDate > moment))
            return Status.Recharging;

        return Status.Idle;
    }

    public enum Status
    {
        Working,
        Recharging,
        Idle
    }

    public static readonly Worker Null = new(Guid.Empty, string.Empty);
}