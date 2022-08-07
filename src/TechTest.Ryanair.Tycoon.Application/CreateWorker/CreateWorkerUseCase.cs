using Awarean.Sdk.Result;

namespace TechTest.Ryanair.Tycoon.Application.CreateWorker
{
    public class CreateWorkerUseCase : ICreateWorkerUseCase
    {
        public Task<Result<CreatedWorkerResponse>> HandleAsync(CreateWorkerCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
