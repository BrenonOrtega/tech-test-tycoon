using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Application.Dtos;

namespace TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;

internal class GetActivityByIdUseCase : IGetActivityByIdUseCase
{
    private readonly IBaseGetByIdUseCase _baseGetById;

    public GetActivityByIdUseCase(IBaseGetByIdUseCase baseGetById)
    {
        _baseGetById = baseGetById ?? throw new ArgumentNullException(nameof(baseGetById));
    }

    public async Task<Result<FoundActivityResponse>> HandleAsync(GetActivityByIdCommand command)
    {
        var getResult = await _baseGetById.HandleAsync(command);

        if (getResult.IsFailed)
            return Result.Fail<FoundActivityResponse>(getResult.Error);

        var dto = ActivityDto.FromEntity(getResult.Value);
        return Result.Success(new FoundActivityResponse(dto));
    }
}
