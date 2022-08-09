using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;

public class GetWorkerByIdCommand : ICommand
{
    public GetWorkerByIdCommand(Guid id) => Id = id;
    
    public Guid Id { get; }
    public Result Validate() => Guid.Empty != Id ? Result.Success() : Result.Fail(ApplicationErrors.InvalidGuid);
}