
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
        public virtual TimeSpan Duration => Start - Finish;
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
            var otherStartsAfter = FinishRestingDate <= other.Start && Start < other.Start;

            var finishesBefore = other.FinishRestingDate <= Start && other.Start < Start;

            return !(otherStartsAfter || finishesBefore);
        }

        public virtual Result HaveParticipant(Worker worker)
        {
            if (worker.Activities.Contains(this) is false)
                return Result.Fail(DomainErrors.InconsistentWorkerInActivity);

            _workers.Add(worker.Id);
            return Result.Success();
        }

        internal virtual Result WorksNoMore(Worker worker)
        {
            if (_workers.Remove(worker.Id))
                return Result.Success();

            throw new InvalidOperationException($"Inconsistent state between activity {JsonSerializer.Serialize(this)} and worker {JsonSerializer.Serialize(worker)}.");
        }

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
        private class NullActivity : TimedActivity
        {
            public NullActivity() : this(Guid.Empty, DateTime.MinValue, DateTime.MaxValue) { }
            private NullActivity(Guid id, DateTime start, DateTime finish) : base(id, start, finish) { }
            public override TimeSpan RestPeriod => TimeSpan.Zero;
            public override string Type => "NULL";
        }
    }
}