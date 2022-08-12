using Awarean.Sdk.Result;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById.Base;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.UnitTests.Application.Activities;

public class GetActivityByIdUseCaseTests
{
    [Fact]
    public async Task Getting_Existent_Activity_Should_Be_Succesful()
    {
        var getById = Substitute.For<IBaseGetByIdUseCase>();
        var sut = (IGetActivityByIdUseCase)new GetActivityByIdUseCase(getById);

        var activity = new BuildComponentActivity(Guid.NewGuid(), new DateTime(), new DateTime().AddHours(1));
        var request = new GetActivityByIdCommand(activity.Id);

        getById.HandleAsync(default).ReturnsForAnyArgs(Result.Success((TimedActivity)activity));

        var result = await sut.HandleAsync(request);
        var dto = result.Value.Activity;

        result.IsSuccess.Should().BeTrue();
        dto.Id.Should().Be(activity.Id);
        dto.Start.Should().Be(activity.Start);
        dto.Finish.Should().Be(activity.Finish);
        dto.Rest.Should().Be(activity.RestPeriod);
        dto.Workers.Should().HaveSameCount(activity.Workers);
        dto.Type.Should().Be(activity.Type);
    }

    [Fact]
    public async Task Getting_Non_Existent_Activity_Should_Fail()
    {
        var getById = Substitute.For<IBaseGetByIdUseCase>();
        var sut = (IGetActivityByIdUseCase)new GetActivityByIdUseCase(getById);

        var request = new GetActivityByIdCommand(Guid.NewGuid());

        getById.HandleAsync(default).ReturnsForAnyArgs(Result.Fail<TimedActivity>(ApplicationErrors.ActivityNotFound));

        var result = await sut.HandleAsync(request);

        result.IsFailed.Should().BeTrue();
        result.Error.Should().Be(ApplicationErrors.ActivityNotFound);
    }

    [Theory]
    [MemberData(nameof(InvalidCommandGenerator))]
    public async Task Invalid_Request_Should_Fail(GetActivityByIdCommand invalid)
    {
        var getById = Substitute.For<IBaseGetByIdUseCase>();
        var sut = (IGetActivityByIdUseCase)new GetActivityByIdUseCase(getById);
        var expectedErrors = new[] { ApplicationErrors.InvalidGuid, ApplicationErrors.NullCommand };

        getById.HandleAsync(default).ReturnsForAnyArgs(Result.Fail<TimedActivity>(ApplicationErrors.InvalidGuid));


        var result = await sut.HandleAsync(invalid);

        result.IsFailed.Should().BeTrue();
        expectedErrors.Should().Contain(result.Error);
    }

    public static IEnumerable<object[]> InvalidCommandGenerator()
    {
        yield return new object[] { new GetActivityByIdCommand(Guid.Empty) };
        yield return new object[] { null };
    }
}
