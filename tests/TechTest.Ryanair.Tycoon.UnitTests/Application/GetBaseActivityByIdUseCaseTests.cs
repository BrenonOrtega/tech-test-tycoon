using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.UnitTests.Application
{
    public class GetBaseActivityByIdUseCaseTests
    {
        [Fact]
        public async Task Getting_Existent_Activity_Should_Be_Succesful()
        {
            var repo = Substitute.For<ITimedActivityRepository>();
            var sut = (IBaseGetByIdUseCase)new BaseGetByIdUseCase(repo);

            var activity = new BuildComponentActivity(Guid.NewGuid(), new DateTime(), new DateTime().AddHours(1));
            var request = new GetActivityByIdCommand(activity.Id);

            repo.GetAsync(Arg.Is(request.Id)).Returns(activity);

            var result = await sut.HandleAsync(request);

            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be(activity.Id);
            result.Value.Start.Should().Be(activity.Start);
            result.Value.Finish.Should().Be(activity.Finish);
            result.Value.RestPeriod.Should().Be(activity.RestPeriod);
            result.Value.Workers.Should().HaveSameCount(activity.Workers);
            result.Value.Type.Should().Be(activity.Type);
        }

        [Fact]
        public async Task Getting_Non_Existent_Activity_Should_Fail()
        {
            var repo = Substitute.For<ITimedActivityRepository>();
            var sut = (IBaseGetByIdUseCase)new BaseGetByIdUseCase(repo);

            var request = new GetActivityByIdCommand(Guid.NewGuid());

            repo.GetAsync(Arg.Is(request.Id)).Returns(TimedActivity.Null);

            var result = await sut.HandleAsync(request);

            result.IsFailed.Should().BeTrue();
            result.Error.Should().Be(ApplicationErrors.ActivityNotFound);
        }

        [Theory]
        [MemberData(nameof(InvalidCommandGenerator))]
        public async Task Invalid_Request_Should_Fail(GetActivityByIdCommand invalid)
        {
            var repo = Substitute.For<ITimedActivityRepository>();
            var sut = (IBaseGetByIdUseCase)new BaseGetByIdUseCase(repo);
            var expectedErrors = new[] { ApplicationErrors.InvalidGuid, ApplicationErrors.NullCommand };

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
}
