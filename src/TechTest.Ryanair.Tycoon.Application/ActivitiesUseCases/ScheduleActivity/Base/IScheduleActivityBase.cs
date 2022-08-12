using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.ScheduleNew;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.Base;

public interface IScheduleActivityBase : IUseCase<ScheduleActivityCommand, TimedActivity>
{

}