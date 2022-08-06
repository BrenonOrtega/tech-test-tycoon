
using System;

namespace TechTest.Ryanair.Tycoon.Domain.Entities
{
    public abstract class TimedActivity
    {
        public virtual string Id { get; set; }
        public virtual string Type { get; set; }
        public virtual List<string> Workers { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime Finish { get; set; }
        public virtual TimeSpan Duration => Start - Finish;
        public abstract TimeSpan RestTime { get; }

        public TimedActivity(DateTime start, DateTime finish)
        {
            if (start > finish)
                throw new ArgumentException("A start can not be after its end.");

            Start = start;
            Finish = finish;
        }

        public bool Overlaps(TimedActivity other)
        {
            var otherStartsAfter = Finish < other.Start && Start < other.Start;

            var finishesBefore = other.Finish < Start && other.Start < Start;

            return !(otherStartsAfter || finishesBefore);
        }
    }
}