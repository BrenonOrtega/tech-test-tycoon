using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.UpdateDates;

public class UpdateActivityDatesCommand : ICommand
{
    public Guid Activity { get; }
    public DateTime NewStartDate { get; }
    public DateTime NewEndDate { get; }

    public UpdateActivityDatesCommand(Guid activity, DateTime newStartDate, DateTime newEndDate)
    {
        Activity = activity;
        NewStartDate = newStartDate;
        NewEndDate = newEndDate;
    }

    public Result Validate()
    {
        var error = new Func<Error, Result>(Result.Fail);
        if (Activity == Guid.Empty)
            return error(ApplicationErrors.InvalidGuid);
        
        if(NewStartDate > NewEndDate)
            return error(ApplicationErrors.InvalidUpdateDate);
        
        return Result.Success();
    }
}
