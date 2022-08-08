using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application;

public interface IUseCase<in TCommand, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command);
}
