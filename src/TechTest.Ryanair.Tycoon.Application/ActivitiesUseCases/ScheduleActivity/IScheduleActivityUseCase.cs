using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateWorker;
public interface IScheduleActivityUseCase
{
    Task<Result<ScheduledActivityResponse>> HandleAsync(ScheduleActivityCommand command);
}
