using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.UpdateDates;

internal class UpdateActivityDatesUseCase : IUpdateActivityDatesUseCase
{
    private readonly ILogger<UpdateActivityDatesUseCase> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateActivityDatesUseCase(ILogger<UpdateActivityDatesUseCase> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<UpdatedActivityDatesResponse>> HandleAsync(UpdateActivityDatesCommand command)
    {
        if (command is null)
        {
            _logger.LogError("Received null command to update a date.");
            return Result.Fail<UpdatedActivityDatesResponse>(ApplicationErrors.NullCommand);
        }

        var validation = command.Validate();

        if(validation.IsFailed)
        {
            var error = validation.Error;
            _logger.LogError("Received invalid update date command error code: {errorCode} - message {errorMessage}.", error.Code, error.Message);
            
            return Result.Fail<UpdatedActivityDatesResponse>(error);
        }

        var activity = await _unitOfWork.ActivityRepository.GetAsync(command.Activity);
        if (activity == TimedActivity.Null)
            return Result.Fail<UpdatedActivityDatesResponse>(ApplicationErrors.ActivityNotFound);

        var workers = await _unitOfWork.WorkerRepository.GetWorkersAsync(activity.Workers);

        var reeschedulingResult = activity.Reeschedule(command.NewStartDate, command.NewEndDate, workers);
        if(reeschedulingResult.IsFailed)
        {
            var error = reeschedulingResult.Error;
            _logger.LogError("Error reescheduling activity id {id}. Details {errorCode} - {errorMessage}.", activity.Id, error.Code, error.Message);
            
            return Result.Fail<UpdatedActivityDatesResponse>(error);
        }

        await _unitOfWork.SaveAsync();
        return Result.Success(new UpdatedActivityDatesResponse(activity.Id, activity.Start, activity.Finish));
    }
}
