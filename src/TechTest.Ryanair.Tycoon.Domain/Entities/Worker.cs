using Awarean.Sdk.Result;
using System.Collections.Immutable;
using TechTest.Ryanair.Tycoon.Domain.FluentApi.WorkerFluent;

namespace TechTest.Ryanair.Tycoon.Domain.Entities;

public class Worker : IActivityWorker
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    private readonly HashSet<TimedActivity> _activities = new();
    public ImmutableHashSet<TimedActivity> Activities => _activities.ToImmutableHashSet();

    public Status ActualStatus
    {
        get
        {
            var moment = DateTime.Now;
            if (_activities.Any(x => x.Start < moment && x.Finish > moment))
                return Status.Working;

            if (_activities.Any(x => x.Finish < moment && x.FinishRestingDate > moment))
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
        if (this == Null)
            return new WorksInResult(DomainErrors.AddingActivityToNullWorker, activity);

        if (activity is null || activity == TimedActivity.Null)
            return new WorksInResult(DomainErrors.TryWorkingInInvalidActivity, TimedActivity.Null);

        if (_activities.Contains(activity))
            return new WorksInResult(DomainErrors.AlreadyWorksInActivity, activity);

        if (_activities.Any(act => act.Overlaps(activity)))
            return new WorksInResult(DomainErrors.OverlappingActivities, activity);

        if(_activities.Any(act => act.OverlapsByRest(activity)))
            return new WorksInResult(DomainErrors.ActivityScheduledInRestTime, activity);

        _activities.Add(activity);
        var result = activity.HaveParticipant(this);

        if (result.IsFailed)
            return new WorksInResult(result.Error, activity);

        return new WorksInResult(this);
    }

    public enum Status
    {
        Working,
        Recharging,
        Idle
    }

    public static readonly Worker Null = new(Guid.Empty, string.Empty);

    public Result Unassign(TimedActivity activity)
    {
        if(_activities.Remove(activity))
        {
            activity.WorksNoMore(this);
            return Result.Success();
        }

        return Result.Fail(DomainErrors.ActivityNotAssignedToWorker);
    }

    public bool CannotWorkNewShift(Guid activityId, DateTime startDate, DateTime endDate)
    {
        var cannotAttend = _activities.Where(x => x.Id != activityId).Any(x => x.WouldOverLapRest(startDate, endDate));

        return cannotAttend;
    }

    internal void GetNewShift(TimedActivity activity)
    {
        Unassign(_activities.Single(x => x.Id == activity.Id));
        WorksIn(activity);
    }
}