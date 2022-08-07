using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.CreateWorker;

public interface ICreateWorkerUseCase
{
    Task<Result<CreatedWorkerResponse>> HandleAsync(CreateWorkerCommand command);
}
