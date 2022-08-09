using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;

public class CreateWorkerCommand : ICommand
{
    public Guid Id { get; init; }
    public string? Name { get; init; }

    public CreateWorkerCommand(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public CreateWorkerCommand() { }

    public Result Validate()
    {
        if (Id == Guid.Empty)
            return Result.Fail("INVALID_ID", "An Invalid Guid was provided to the command");

        if (string.IsNullOrEmpty(Name))
            return Result.Fail("INVALID_NAME", "An Invalid name was provided to the command");

        return Result.Success();
    }
}
