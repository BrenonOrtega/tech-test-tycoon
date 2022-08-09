using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.UnitTests.Application;

public class CreateActivityUseCaseTests
{
    [Theory]
    [InlineData("Component")]
    [InlineData("Machine")]
    public async Task Creating_Activity_Should_Pass(string activityType)
    {
        // Given
        var logger = Substitute.For<ILogger<CreateActivityUseCase>>();
        var repo = Substitute.For<ITimedActivityRepository>();
        var sut = (ICreateActivityUseCase)new CreateActivityUseCase(logger, repo);
        var command = new CreateActivityCommand(
            id: Guid.NewGuid(), 
            activityType: activityType,  
            startDate: DateTime.Today,
            finishDate: DateTime.Today.AddDays(1));

        // When
        var result = await sut.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(command.Id);
        result.Value.StartDate.Should().Be(command.StartDate);
        result.Value.FinishDate.Should().Be(command.FinishDate);
        result.Value.Type.Should().NotBeNull().And.NotBe(string.Empty);
        result.Value.RestTime.Should().NotBeNull().And.NotBe(string.Empty);
    }
}
