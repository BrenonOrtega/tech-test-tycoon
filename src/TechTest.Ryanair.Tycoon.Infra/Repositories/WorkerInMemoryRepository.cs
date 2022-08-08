using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Infra.Repositories
{
    internal class WorkerInMemoryRepository : BaseInMemoryRepository<Worker, WorkerInMemoryRepository>, IWorkerRepository
    {
        public WorkerInMemoryRepository(Dictionary<Guid, Worker> workers, ILogger<WorkerInMemoryRepository> logger) : base(logger)
        {
            _workers = workers ?? throw new ArgumentNullException(nameof(workers));
        }

        private readonly Dictionary<Guid, Worker> _workers;
        protected override Dictionary<Guid, Worker> Data => _workers;

        public async Task<Worker> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Worker.Null;

            var worker = _workers.Where(tuple => name.Equals(tuple.Value?.Name)).FirstOrDefault().Value;

            if (worker is null)
                return Worker.Null;

            return worker;
        }

        public async Task<IEnumerable<Worker>> GetWorkersAsync(IEnumerable<Guid> workerIds)
        {
            if (workerIds.Any() is false)
                return Enumerable.Empty<Worker>();

            var workers = _workers.Where(pair => workerIds.Contains(pair.Value.Id))
                .Select(pair => pair.Value)
                .ToList();

            return workers;
        }
    }
}
