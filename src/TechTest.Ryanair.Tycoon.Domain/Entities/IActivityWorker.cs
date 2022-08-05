using TechTest.Ryanair.Tycoon.Domain.FluentApi.Activity;

namespace TechTest.Ryanair.Tycoon.Domain.Entities;

    public interface IActivityWorker
    {
        WorksInResult WorksIn(Activity activity);
    }
