using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Domain.Entities
{
    public class BuildComponentActivity : TimedActivity
    {
        public BuildComponentActivity(Guid id, DateTime start, DateTime finish) : base(id, start, finish)
        {
        }

        public override TimeSpan RestPeriod => TimeSpan.FromHours(2);

        public override string Type => "COMPONENT";

        public override Result HaveParticipant(Worker worker)
        {
            if (_workers.Count == 1)
                return Result.Fail(DomainErrors.InvalidActivityAssignment);

            return base.HaveParticipant(worker);
        }
    }
}