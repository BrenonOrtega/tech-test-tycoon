using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;

public class FoundWorkerResponse
{
    private FoundWorkerResponse(Guid id, string name, Worker.Status status, IEnumerable<TimedActivity> activities) 
    {
        Id = id;
        Name = name;
        Status = status;
        Activities = activities;
    }
    public FoundWorkerResponse(Worker queried) 
        : this(queried.Id, queried.Name, queried.ActualStatus, queried.Activities)
    {
    }

    public Guid Id { get; }
    public string Name { get; }
    public Worker.Status Status { get; }
    public IEnumerable<TimedActivity> Activities { get; }

    public static readonly FoundWorkerResponse Null = new(Guid.Empty, "None", Worker.Status.Idle, Enumerable.Empty<TimedActivity>());
}