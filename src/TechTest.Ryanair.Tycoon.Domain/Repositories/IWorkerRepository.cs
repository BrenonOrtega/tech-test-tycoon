using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Domain.Repositories
{
    public interface IWorkerRepository
    {
        Task<Worker> GetAsync(Guid id);
        Task<Worker> GetByName(string name);
        Task<Result> CreateAsync(Worker worker);
        Task<Result> UpdateAsync(Guid id, Worker updated);
    }
}