using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.AssignExistent;

namespace TechTest.Ryanair.Tycoon.Api.Requests;

public class AssignExistentActivityRequest
{
    public Guid ActivityId { get; init; }
    public List<Guid> WorkerIds { get; init;  }

    public AssignExistentActivityCommand ToCommand() => new(ActivityId, WorkerIds);
}
