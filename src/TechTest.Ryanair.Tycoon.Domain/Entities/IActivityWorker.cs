using TechTest.Ryanair.Tycoon.Domain.FluentApi.Worker;

namespace TechTest.Ryanair.Tycoon.Domain.Entities;

    public interface IActivityWorker
    {
        WorksInResult WorksIn(TimedActivity activity);
    }
