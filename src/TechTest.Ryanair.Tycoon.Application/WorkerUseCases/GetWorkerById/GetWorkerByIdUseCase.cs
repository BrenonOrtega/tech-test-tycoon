using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;

internal class GetWorkerByIdUseCase : IGetWorkerByIdUseCase
{
    private readonly IWorkerRepository _workers;

    public GetWorkerByIdUseCase(IWorkerRepository workers)
    {
        _workers = workers ?? throw new ArgumentNullException(nameof(workers));
    }

    public Task<Result<FoughtWorkerResponse>> HandleAsync(GetWorkerByIdCommand command)
    {
        throw new NotImplementedException();
    }
}
