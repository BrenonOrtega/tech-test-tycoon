using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;

public interface ICreateWorkerUseCase
{
    Task<Result<CreatedWorkerResponse>> HandleAsync(CreateWorkerCommand command);
}
