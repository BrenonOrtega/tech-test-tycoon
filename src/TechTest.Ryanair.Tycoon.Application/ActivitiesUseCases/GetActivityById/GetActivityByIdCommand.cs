using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;

public class GetActivityByIdCommand : ICommand
{
    public GetActivityByIdCommand(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; }

    public Result Validate() => Id != Guid.Empty ? Result.Success() : Result.Fail(ApplicationErrors.InvalidGuid);
}