using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;

internal class BaseGetByIdUseCase : IBaseGetByIdUseCase
{
    private readonly ITimedActivityRepository _repo;

    public BaseGetByIdUseCase(ITimedActivityRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public async Task<Result<TimedActivity>> HandleAsync(GetActivityByIdCommand command)
    {
        if (command is null)
            return Result.Fail<TimedActivity>(ApplicationErrors.NullCommand);

        var validation = command.Validate();

        if (validation.IsFailed)
            return Result.Fail<TimedActivity>(validation.Error);

        var queriedActivity = await _repo.GetAsync(command.Id);

        if (queriedActivity == TimedActivity.Null)
            return Result.Fail<TimedActivity>(ApplicationErrors.ActivityNotFound);

        return Result.Success(queriedActivity);
    }
}
