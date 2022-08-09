using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;

public class CreateActivityCommand : ICommand
{
    public Guid Id { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime FinishDate { get; private set; }
    public string ActivityType { get; private set; }

    public CreateActivityCommand(Guid id, string activityType, DateTime startDate, DateTime finishDate)
    {
        Id = id;
        ActivityType = activityType.ToUpper();
        StartDate = startDate;
        FinishDate = finishDate;
    }

    public Result Validate()
    {
        var validId = Id != Guid.Empty;
        var validPeriod = StartDate < FinishDate;
        var validType = string.IsNullOrEmpty(ActivityType) is false;

        return validId && validPeriod && validType
            ? Result.Success()
            : Result.Fail(ApplicationErrors.InvalidCommand);
    }
}