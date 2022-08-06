using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Domain.FluentApi.Activity
{
    public class WorksInResult : Result<IActivityWorker>, IActivityWorker
    {
        public TimedActivity FailedActivity { get; }
        public WorksInResult(IActivityWorker worker) : base(worker) { }

        public WorksInResult(Error error, TimedActivity failedActivity) : base(error)
            => FailedActivity = failedActivity ?? throw new ArgumentNullException(nameof(failedActivity));

        public WorksInResult WorksIn(TimedActivity activity)
        {
            if(IsFailed)
                return this;
            
            return Value.WorksIn(activity);
        }
    }
}