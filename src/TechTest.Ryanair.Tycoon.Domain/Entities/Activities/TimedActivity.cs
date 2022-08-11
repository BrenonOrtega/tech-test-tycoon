
using Awarean.Sdk.Result;
using System.Collections.Immutable;
using System.Text.Json;

namespace TechTest.Ryanair.Tycoon.Domain.Entities
{
    public abstract class TimedActivity : IEquatable<TimedActivity>
    {
        public virtual Guid Id { get; protected set; }
        protected readonly HashSet<Guid> _workers = new();
        public virtual ImmutableHashSet<Guid> Workers => _workers.ToImmutableHashSet();
        public virtual DateTime Start { get; protected set; }
        public virtual DateTime Finish { get; protected set; }
        public virtual TimeSpan Duration => Finish - Start;
        public virtual DateTime FinishRestingDate => Finish + RestPeriod;
        public abstract TimeSpan RestPeriod { get; }
        public abstract string Type { get; }

        public TimedActivity(Guid id, DateTime start, DateTime finish)
        {
            if (start >= finish)
                throw new ArgumentException("A start can not be after its end.");

            Id = id;
            Start = start;
            Finish = finish;
        }

        public bool Overlaps(TimedActivity other)
        {
            var otherStartsAfter = StartsAfter(other.Start);

            var finishesBefore = EndsAfter(other.Start, other.Finish);
            return IsOverlapping(otherStartsAfter, finishesBefore);
        }

        private static bool IsOverlapping(bool otherStartsAfter, bool finishesBefore) => !(otherStartsAfter || finishesBefore);

        public bool OverlapsByRest(TimedActivity other)
        {
            var otherStartsAfter = StartsAfterRest(other.Start);
            var finishesBefore = EndsAfterRest(other.Start, other.FinishRestingDate);

            return IsOverlapping(otherStartsAfter, finishesBefore);
        }


        public virtual Result HaveParticipant(Worker worker)
        {
            if (worker.Activities.Contains(this) is false)
                return Result.Fail(DomainErrors.InconsistentWorkerInActivity);

            _workers.Add(worker.Id);
            return Result.Success();
        }

        public Result Reeschedule(DateTime newStartDate, DateTime newEndDate, IEnumerable<Worker> workers)
        {
            if (newStartDate > newEndDate)
                return Result.Fail(DomainErrors.InvalidReeschedulingDates);

            if (WorkersDoesntMatch(workers))
                return Result.Fail(DomainErrors.InvalidReeschedulingWorkers);

            var cannotAttend = workers.Any(x => x.CannotWorkNewShift(Id, newStartDate, newEndDate));

            if (cannotAttend)
                return Result.Fail(DomainErrors.OverlappingActivities);

            Start = newStartDate;
            Finish = newEndDate;

            foreach (var worker in workers)
                worker.GetNewShift(this);

            return Result.Success();
        }

        private bool WorkersDoesntMatch(IEnumerable<Worker> workers) 
            => workers.Count() != _workers.Count || workers.All(x => _workers.Contains(x.Id)) is false;

        internal virtual Result WorksNoMore(Worker worker)
        {
            if (_workers.Remove(worker.Id))
                return Result.Success();

            return Result.Fail(DomainErrors.InconsistentWorkerInActivity);
        }

        internal bool StartsAfter(DateTime otherStart) => Finish <= otherStart && Start < otherStart;
        internal bool EndsAfter(DateTime otherStart, DateTime otherEnd) => otherEnd <= Start && otherStart < Start;
        internal bool WouldOverLap(DateTime startDate, DateTime endDate) => IsOverlapping(StartsAfter(startDate), EndsAfter(startDate, endDate));
        internal bool StartsAfterRest(DateTime otherStart) => FinishRestingDate <= otherStart && Start < otherStart;
        internal bool EndsAfterRest(DateTime otherStart, DateTime otherEndRestingDate) => otherEndRestingDate <= Start && otherStart < Start;
        internal bool WouldOverLapRest(DateTime startDate, DateTime endRestDate) => IsOverlapping(StartsAfterRest(startDate), EndsAfterRest(startDate, endRestDate));

        public bool Equals(TimedActivity? other)
        {
            if (other is null)
                return false;

            var IdEquals = other.Id == Id;
            var startsEqual = other.Start == Start;
            var finishEquals = other.Finish == Finish;
            var restingDateEquals = other.FinishRestingDate == FinishRestingDate;
            var restPeriodEquals = other.RestPeriod == RestPeriod;

            return IdEquals && startsEqual && finishEquals && restingDateEquals && restPeriodEquals;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null)
                return false;

            if (obj is not TimedActivity act)
                return false;

            return Equals(act);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode()
                + Start.GetHashCode()
                + Finish.GetHashCode()
                + FinishRestingDate.GetHashCode()
                + RestPeriod.GetHashCode();
        }

        public static readonly TimedActivity Null = new NullActivity();
        private sealed class NullActivity : TimedActivity
        {
            public NullActivity() : this(Guid.Empty, DateTime.MinValue, DateTime.MaxValue) { }
            private NullActivity(Guid id, DateTime start, DateTime finish) : base(id, start, finish) { }
            public override TimeSpan RestPeriod => TimeSpan.Zero;
            public override string Type => "NULL";
        }
    }
}