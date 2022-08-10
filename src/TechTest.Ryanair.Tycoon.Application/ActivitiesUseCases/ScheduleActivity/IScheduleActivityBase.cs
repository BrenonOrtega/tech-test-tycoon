using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity
{
    internal interface IScheduleActivityBase : IUseCase<ScheduleActivityCommand, (TimedActivity activity, IEnumerable<Worker> workers)>
    {
        
    }
}