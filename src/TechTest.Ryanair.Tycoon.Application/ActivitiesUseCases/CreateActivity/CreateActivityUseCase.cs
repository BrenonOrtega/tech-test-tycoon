using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Factories;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;

internal class CreateActivityUseCase : ICreateActivityUseCase
{
    private ILogger<CreateActivityUseCase> _logger;
    private ITimedActivityRepository _activities;
    private readonly IActivityFactory _factory;

    public CreateActivityUseCase(ILogger<CreateActivityUseCase> logger, ITimedActivityRepository activities, IActivityFactory factory)
    {
        _logger = logger;
        _activities = activities;
        _factory = factory;
    }

    public async Task<Result<CreatedActivityResponse>> HandleAsync(CreateActivityCommand command)
    {
        if (command is null)
            return Result.Fail<CreatedActivityResponse>(ApplicationErrors.NullCommand);

        var validation = command.Validate();
        if (validation.IsFailed)
            return Result.Fail<CreatedActivityResponse>(validation.Error);

        var existent = await _activities.GetAsync(command.Id);

        if (existent != TimedActivity.Null)
            return Result.Fail<CreatedActivityResponse>(ApplicationErrors.DuplicatedId);

        var newActivity = _factory.FromCreateCommand(command);
        var result = await _activities.CreateAsync(newActivity);

        if(result.IsFailed)
            return Result.Fail<CreatedActivityResponse>(result.Error);

        return Result.Success(
            new CreatedActivityResponse(newActivity.Id, 
                                        newActivity.Start, 
                                        newActivity.Finish, 
                                        newActivity.RestPeriod.ToString(),  
                                        newActivity.Type)
        );
    }
}