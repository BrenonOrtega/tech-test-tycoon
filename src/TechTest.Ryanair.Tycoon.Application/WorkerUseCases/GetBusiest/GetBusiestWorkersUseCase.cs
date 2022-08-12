using Awarean.Sdk.Result;
using Microsoft.Extensions.Options;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;

internal class GetBusiestWorkersUseCase : UseCaseBase<IEnumerable<Worker>>, IGetBusiestWorkersUseCase
{
    private readonly IWorkerRepository _repo;
    private readonly GetBusiestWorkersOptions _options;

    public GetBusiestWorkersUseCase(IWorkerRepository repo, IOptionsSnapshot<GetBusiestWorkersOptions> options)
    {
        _repo = repo;
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<Result<IEnumerable<Worker>>> HandleAsync(GetBusiestWorkersCommand command)
    {
        if (command is null)
            return NullCommand;

        var validation = command.Validate();

        if (validation.IsFailed)
            command = UseDefault();

        var busiestWorkers = await _repo.GetAllAsync(x => x.WorkTimeBetween(command.StartDate, command.FinalDate) > TimeSpan.Zero, skip: 0, take: command.Count);

        if (busiestWorkers is null) 
            return Result.Success(Enumerable.Empty<Worker>());

        return Result.Success(busiestWorkers);
    }

    private GetBusiestWorkersCommand UseDefault() => new((int)_options.GetQuantity,(DateTime)_options.StartDate, (DateTime)_options.FinishDate);
}

internal abstract class UseCaseBase<TResponse>
{
    public static Result<TResponse> NullCommand => Result.Fail<TResponse>(ApplicationErrors.NullCommand);
    public static Result<TResponse> InvalidCommand(Result validation) => Result.Fail<TResponse>(validation.Error);
}
