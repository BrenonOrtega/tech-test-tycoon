using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.ScheduleActivity;
public interface IScheduleActivityUseCase
{
    Task<Result<ScheduledActivityResponse>> HandleAsync(ScheduleActivityCommand command);
}
