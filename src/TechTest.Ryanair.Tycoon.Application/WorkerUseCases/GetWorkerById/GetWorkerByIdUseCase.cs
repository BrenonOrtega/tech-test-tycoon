using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;

internal class GetWorkerByIdUseCase : IGetWorkerByIdUseCase
{
    private readonly IWorkerRepository _workers;

    public GetWorkerByIdUseCase(IWorkerRepository workers)
    {
        _workers = workers ?? throw new ArgumentNullException(nameof(workers));
    }

    public async Task<Result<FoundWorkerResponse>> HandleAsync(GetWorkerByIdCommand command)
    {
        if (command is null)
            return Result.Fail<FoundWorkerResponse>(ApplicationErrors.NullCommand);

        var validation = command.Validate();
        if (validation.IsFailed)
            return Result.Fail<FoundWorkerResponse>(validation.Error);

        var queried = await _workers.GetAsync(command.Id);

        if(queried == Worker.Null)
            return Result.Fail<FoundWorkerResponse>(ApplicationErrors.WorkerNotFound);

        return Result.Success(new FoundWorkerResponse(queried));
    }
}
