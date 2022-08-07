using TechTest.Ryanair.Tycoon.Domain.FluentApi.WorkerFluent;

namespace TechTest.Ryanair.Tycoon.Domain.Entities;

public interface IActivityWorker
{
    WorksInResult WorksIn(TimedActivity activity);
}
