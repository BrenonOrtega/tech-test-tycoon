
using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Domain.Repositories;

public interface IUnitOfWork
{
    IWorkerRepository WorkerRepository { get; }
    ITimedActivityRepository ActivityRepository { get; }
    Task<Result> SaveAsync();
}