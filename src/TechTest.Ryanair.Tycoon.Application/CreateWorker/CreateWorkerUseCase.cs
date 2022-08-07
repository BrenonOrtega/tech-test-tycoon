using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Domain.Entities;
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

        public async Task<Result<CreatedWorkerResponse>> HandleAsync(CreateWorkerCommand command)
        {
            if (command is null)
                return Result.Fail<CreatedWorkerResponse>(ApplicationErrors.NullCommand);

            if (command.Validate().IsFailed)
                return Result.Fail<CreatedWorkerResponse>(ApplicationErrors.InvalidCommand);

            _logger.LogInformation("Creating worker {id}, name {name}", command.Id, command.Name);
            await _unitOfWork.WorkerRepository.CreateAsync(new Worker(command.Id, command.Name));

            return Result.Success(new CreatedWorkerResponse(command.Id));
        }
    }
}
