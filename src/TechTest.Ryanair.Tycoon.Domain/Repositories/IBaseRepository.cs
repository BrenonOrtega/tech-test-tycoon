using Awarean.Sdk.Result;
using System.Linq.Expressions;

namespace TechTest.Ryanair.Tycoon.Domain.Repositories;

public interface IBaseRepository<T>
{
    Task<T> GetAsync(Guid id);
    Task<Result> CreateAsync(T entity);
    Task<Result> UpdateAsync(Guid id, T updated);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, int skip=0, int take = 100);
}
