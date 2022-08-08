using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.UseCase;

public interface IUseCase<in TCommand, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command);
}
