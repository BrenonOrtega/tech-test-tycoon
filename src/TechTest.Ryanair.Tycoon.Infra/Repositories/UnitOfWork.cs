using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Infra.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly IWorkerRepository _workerRepo;
        public IWorkerRepository WorkerRepository => _workerRepo;

        private readonly ITimedActivityRepository _activityRepo;
        public ITimedActivityRepository ActivityRepository => _activityRepo;

        public UnitOfWork(ITimedActivityRepository activityRepo, IWorkerRepository workerRepo)
        {
            _activityRepo = activityRepo;
            _workerRepo = workerRepo;
        }

        public async Task<Result> SaveAsync()
        {
            return Result.Success();
        }
    }
}
