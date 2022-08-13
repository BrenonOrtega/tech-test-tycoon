using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;

public class GetWeeklyTopTenBusiestWorkersCommand : GetBusiestWorkersCommand
{
    public GetWeeklyTopTenBusiestWorkersCommand() : base(10, DateTime.Now.AddDays(-7), DateTime.Now)
    {
    }
}