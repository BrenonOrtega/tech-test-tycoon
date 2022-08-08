using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Domain.Repositories
{
    public interface IWorkerRepository : IBaseRepository<Worker>
    {
        Task<Worker> GetByNameAsync(string name);
        Task<IEnumerable<Worker>> GetWorkersAsync(IEnumerable<Guid> workerIds);
    }
}