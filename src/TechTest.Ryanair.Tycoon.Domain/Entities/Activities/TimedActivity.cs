
using System;

namespace TechTest.Ryanair.Tycoon.Domain.Entities
{
    public abstract class TimedActivity
    {
        public virtual Guid Id { get; protected set; }
        public virtual List<string> Workers { get; protected set; }
        public virtual DateTime Start { get; protected set; }
        public virtual DateTime Finish { get; protected set; }
        public virtual TimeSpan Duration => Start - Finish;
        public virtual DateTime FinishRestingDate => Finish + RestTime;
        public abstract TimeSpan RestTime { get; }
        public abstract string Type { get; }

        public TimedActivity(Guid id, DateTime start, DateTime finish)
        {
            if (start > finish)
                throw new ArgumentException("A start can not be after its end.");

            Start = start;
            Finish = finish;
        }

        public bool Overlaps(TimedActivity other)
        {
            var otherStartsAfter = FinishRestingDate <= other.Start && Start < other.Start;

            var finishesBefore = other.FinishRestingDate <= Start && other.Start < Start;

            return !(otherStartsAfter || finishesBefore);
        }
    }
}