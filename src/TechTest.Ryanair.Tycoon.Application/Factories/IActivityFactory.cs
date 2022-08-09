using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Domain.Factories
{
    public interface IActivityFactory
    {
        TimedActivity FromCreateCommand(CreateActivityCommand command);
    }
}