using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;

public interface IGetBusiestWorkersUseCase : IUseCase<GetBusiestWorkersCommand, IEnumerable<Worker>>, IUseCase<GetWeeklyTopTenBusiestWorkersCommand, IEnumerable<Worker>>
{
}