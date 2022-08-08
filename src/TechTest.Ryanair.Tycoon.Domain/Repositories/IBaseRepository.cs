using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Domain.Repositories;

public interface IBaseRepository<T>
{
    Task<T> GetAsync(Guid id);
    Task<Result> CreateAsync(T TimedActivity);
    Task<Result> UpdateAsync(Guid id, T updated);
}