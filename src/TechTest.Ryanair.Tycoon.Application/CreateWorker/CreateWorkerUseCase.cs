using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.CreateWorker
{
    public class CreateWorkerUseCase : ICreateWorkerUseCase
    {
        private ILogger<CreateWorkerUseCase> _logger;
        private IUnitOfWork _unitOfWork;

        public CreateWorkerUseCase(ILogger<CreateWorkerUseCase> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Result<CreatedWorkerResponse>> HandleAsync(CreateWorkerCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
