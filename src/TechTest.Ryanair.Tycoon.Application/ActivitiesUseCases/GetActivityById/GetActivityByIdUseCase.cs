using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Application.Dtos;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;

internal class GetActivityByIdUseCase : IGetActivityByIdUseCase
{
    private readonly ITimedActivityRepository _repo;

    public GetActivityByIdUseCase(ITimedActivityRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public async Task<Result<FoundActivityResponse>> HandleAsync(GetActivityByIdCommand command)
    {
        if (command is null)
            return Result.Fail<FoundActivityResponse>(ApplicationErrors.NullCommand);

        var validation = command.Validate();

        if (validation.IsFailed)
            return Result.Fail<FoundActivityResponse>(validation.Error);

        var queriedActivity = await _repo.GetAsync(command.Id);

        if (queriedActivity == TimedActivity.Null)
            return Result.Fail<FoundActivityResponse>(ApplicationErrors.ActivityNotFound);

        var dto = ActivityDto.FromEntity(queriedActivity);
        return Result.Success(new FoundActivityResponse(dto));
    }
}
