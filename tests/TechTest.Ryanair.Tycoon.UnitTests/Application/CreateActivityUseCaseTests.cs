using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Factories;
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
        var factory = new ActivityFactory();
        var sut = (ICreateActivityUseCase)new CreateActivityUseCase(logger, repo, factory);
        var command = new CreateActivityCommand(
            id: Guid.NewGuid(),
            activityType: activityType,
            startDate: DateTime.Today,
            finishDate: DateTime.Today.AddDays(1));

        repo.GetAsync(Arg.Any<Guid>()).Returns(TimedActivity.Null);
        repo.CreateAsync(Arg.Is<TimedActivity>(x => x.Id == command.Id)).ReturnsForAnyArgs(Result.Success());

        // When
        var result = await sut.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(command.Id);
        result.Value.StartDate.Should().Be(command.StartDate);
        result.Value.FinishDate.Should().Be(command.FinishDate);
        result.Value.Type.Should().NotBeNull().And.NotBe(string.Empty);
        result.Value.RestPeriod.Should().NotBeNull().And.NotBe(string.Empty);
    }

    [Theory]
    [InlineData("Component")]
    [InlineData("Machine")]
    public async Task Duplicated_Activity_Ids_Should_fail(string activityType)
    {
        // Given
        var logger = Substitute.For<ILogger<CreateActivityUseCase>>();
        var repo = Substitute.For<ITimedActivityRepository>();
        var factory = new ActivityFactory();
        var sut = (ICreateActivityUseCase)new CreateActivityUseCase(logger, repo, factory);

        var command = new CreateActivityCommand(
            id: Guid.NewGuid(),
            activityType: activityType,
            startDate: DateTime.Today,
            finishDate: DateTime.Today.AddDays(1));

        repo.GetAsync(Arg.Any<Guid>()).Returns(new BuildComponentActivity(command.Id, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(1)));

        // When
        var result = await sut.HandleAsync(command);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(ApplicationErrors.DuplicatedId);
    }

    [Theory]
    [InlineData("Component")]
    [InlineData("Machine")]
    public async Task Failing_Saving_Activity(string activityType)
    {
        // Given
        var logger = Substitute.For<ILogger<CreateActivityUseCase>>();
        var repo = Substitute.For<ITimedActivityRepository>();
        var factory = new ActivityFactory();
        var sut = (ICreateActivityUseCase)new CreateActivityUseCase(logger, repo, factory);

        var command = new CreateActivityCommand(
            id: Guid.NewGuid(),
            activityType: activityType,
            startDate: DateTime.Today,
            finishDate: DateTime.Today.AddDays(1));

        var expectedError = Result.Fail("TestFailure", "failed for test");

        repo.GetAsync(Arg.Any<Guid>()).Returns(TimedActivity.Null);
        repo.CreateAsync(Arg.Is<TimedActivity>(x => x.Id == command.Id)).ReturnsForAnyArgs(expectedError);

        // When
        var result = await sut.HandleAsync(command);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(expectedError.Error);
    }
}
