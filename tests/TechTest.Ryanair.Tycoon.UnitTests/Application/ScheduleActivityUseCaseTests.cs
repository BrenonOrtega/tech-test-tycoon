using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.ScheduleActivity;
using TechTest.Ryanair.Tycoon.Domain;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.UnitTests.Application;

public class ScheduleActivityUseCaseTests
{
    private readonly IUnitOfWork unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task Creating_Activity_That_Workers_Can_Do_Should_Be_Success()
    {
        // Given
        var sut = GetMockedUseCase();

        var workers = new List<Worker> { new(Guid.NewGuid(), "A"), new(Guid.NewGuid(), "B") };
        var activity = new BuildComponentActivity(Guid.NewGuid(), new DateTime(2022, 08, 07), new DateTime(2022, 08, 08));
        var otherActivity = new BuildMachineActivity(Guid.NewGuid(), activity.FinishRestingDate.AddSeconds(1), activity.FinishRestingDate.AddMinutes(30));
        var command = new ScheduleActivityCommand(otherActivity, workers.Select(x => x.Id).ToArray());

        // Setup
        workers.ForEach(worker => worker.WorksIn(activity));
        unitOfWork.WorkerRepository.GetWorkersAsync(default).ReturnsForAnyArgs(workers);

        // When
        var result = await sut.HandleAsync(command);

        // Then
        result.IsSuccess.Should().BeTrue();
        await unitOfWork.Received().ActivityRepository.CreateAsync(Arg.Is(command.Activity));
        await unitOfWork.Received().WorkerRepository.UpdateAsync(Arg.Any<Guid>(), Arg.Any<Worker>());
    }

    [Fact]
    public async Task Scheduling_Activity_Without_Overlaping_Should_Work()
    {
        var sut = GetMockedUseCase();

        var worker = new Worker(Guid.NewGuid(), "B");

        var command = new ScheduleActivityCommand(
            activity: new BuildMachineActivity(Guid.NewGuid(), DateTime.Today.AddDays(-1), DateTime.Now.AddHours(1)),
            assignedWorkers: worker.Id
        );

        unitOfWork.WorkerRepository
            .GetWorkersAsync(Arg.Is<IEnumerable<Guid>>(x => x.Any(c => c == worker.Id)))
            .ReturnsForAnyArgs(new List<Worker>() { worker });

        var result = await sut.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.ActivityId.Should().Be(command.Activity.Id);
    }

    [Theory]
    [MemberData(nameof(WorkerGenerator))]
    public async Task Creating_Activity_That_Overlaps_Any_Worker_Should_Fail(List<Worker> workers, params int[] workersIndex)
    {
        // Given
        var sut = GetMockedUseCase();
                
        var activity = new BuildComponentActivity(Guid.NewGuid(), new DateTime(2022, 08, 07), new DateTime(2022, 08, 08));
        var overlaping = new BuildMachineActivity(Guid.NewGuid(), new DateTime(2022, 08, 07, 10, 15, 00), new DateTime(2022, 08, 07, 11, 15, 00));
        var command = new ScheduleActivityCommand(overlaping, workers.Select(x => x.Id).ToArray());
        
        // Setup
        foreach (var index in workersIndex) 
            workers[index].WorksIn(activity);
        unitOfWork.WorkerRepository.GetWorkersAsync(default).ReturnsForAnyArgs(workers);

        // When
        var result = await sut.HandleAsync(command);

        // Then
        result.IsFailed.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.OverlappingActivities);
        await unitOfWork.WorkerRepository.DidNotReceiveWithAnyArgs().UpdateAsync(default, default);
        await unitOfWork.ActivityRepository.DidNotReceiveWithAnyArgs().CreateAsync(default);
        await unitOfWork.DidNotReceiveWithAnyArgs().SaveAsync();
    }


    [Theory]
    [MemberData(nameof(InvalidCommandGenerator))]
    public async Task Invalid_Command_Should_Fail(ScheduleActivityCommand invalidCommand)
    {
        //Given
        var sut = GetMockedUseCase();
        var acceptedErrors = new[] { ApplicationErrors.InvalidScheduleActivityCommand, ApplicationErrors.InvalidCommand };

        // When
        var result = await sut.HandleAsync(invalidCommand);

        // Then 
        result.IsSuccess.Should().BeFalse();
        acceptedErrors.Should().Contain(result.Error);
    }

    private IScheduleActivityUseCase GetMockedUseCase()
    {
        var logger = Substitute.For<ILogger<ScheduleActivityUseCase>>();
        var sut = (IScheduleActivityUseCase)new ScheduleActivityUseCase(logger, unitOfWork);

        return sut;
    }

    public static IEnumerable<object[]> WorkerGenerator()
    {
        yield return new object[] { new List<Worker> { new(Guid.NewGuid(), "A") }, new[] { 0 } };
        yield return new object[] { new List<Worker> { new(Guid.NewGuid(), "A"), new(Guid.NewGuid(), "B") }, new[] { 1 } };
        yield return new object[] { new List<Worker> { new(Guid.NewGuid(), "A"), new(Guid.NewGuid(), "B"), new(Guid.NewGuid(), "C") }, new[] { 1, 2 } };
        yield return new object[] { new List<Worker> { new(Guid.NewGuid(), "A"), new(Guid.NewGuid(), "B"), new(Guid.NewGuid(), "C") }, new[] { 0, 2 } };
        yield return new object[] { new List<Worker> { new(Guid.NewGuid(), "A"), new(Guid.NewGuid(), "B"), new(Guid.NewGuid(), "C") }, new[] { 0 } };
    }

    public static IEnumerable<object[]> InvalidCommandGenerator()
    {
        yield return new object[] { null };
        yield return new object[] { new ScheduleActivityCommand(new BuildComponentActivity(Guid.NewGuid(), DateTime.Today.AddDays(-1), DateTime.Now.AddHours(1)), null) };
        yield return new object[] { new ScheduleActivityCommand(new BuildComponentActivity(Guid.NewGuid(), DateTime.Today.AddDays(-1), DateTime.Now.AddHours(1)), Guid.Empty) };
        yield return new object[] { new ScheduleActivityCommand(null, Guid.NewGuid()) };
        yield return new object[] { new ScheduleActivityCommand(TimedActivity.Null, Guid.NewGuid()) };
    }
}
