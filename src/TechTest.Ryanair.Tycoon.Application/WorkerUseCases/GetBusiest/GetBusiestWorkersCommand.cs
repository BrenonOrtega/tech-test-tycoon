using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;


public class GetBusiestWorkersCommand : ICommand
{
    public GetBusiestWorkersCommand(int count, DateTime startDate, DateTime finalDate)
    {
        Count = count;
        StartDate = startDate;
        FinalDate = finalDate;
    }

    public int Count { get;  }
    public DateTime StartDate { get; }
    public DateTime FinalDate { get; }

    public Result Validate()
    {
        return Count >= 0 && FinalDate > StartDate ? Result.Success() : Result.Fail(ApplicationErrors.InvalidCommand);
    }
}
