using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Domain.Factories
{
    public class ActivityFactory : IActivityFactory
    {
        // This could be removed when activities parametrized from a database, or from configuration are implemented.
        public TimedActivity FromCreateCommand(CreateActivityCommand command) => command.ActivityType.ToUpper() switch
        {
            "MACHINE" => new BuildMachineActivity(command.Id, command.StartDate, command.FinishDate),
            "COMPONENT" => new BuildComponentActivity(command.Id, command.StartDate, command.FinishDate),
            _ => throw new NotSupportedException($"Cannot create an activity for type {command.ActivityType}, it does not exist in system.")
        };
    }
}
