using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.UpdateDates;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.UnitTests.Application;

public class UpdateActivityUseCaseTests
{
    [Fact]
    public void Updating_An_Activity_Date_Without_Overlapping_Should_Be_Success()
    {
        // Given
        var logger = Substitute.For<ILogger<UpdateActivityDatesUseCase>>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var sut = (IUpdateActivityDatesUseCase)new UpdateActivityDatesUseCase(logger, unitOfWork);

        var workerA = new Worker(Guid.NewGuid(), "A");
        var workerB = new Worker(Guid.NewGuid(), "B");
        var activityA = new BuildMachineActivity(Guid.NewGuid(), new DateTime(2022, 10, 20, 10, 10, 00), new DateTime(2022, 10, 20, 10, 10, 00));


        workerA.WorksIn(activityA);
        workerB.WorksIn(activityA);

        var command = new UpdateActivityDatesCommand(activity: activityA.Id, newStartDate: new DateTime(2022, 10, 09), newEndDate: new DateTime(2022, 10, 11));
        // When
    
        // Then
    }

    [Fact]
    public async void Updating_An_Activity_Date_With_Overlapping_Should_Be_Fail()
    {
        // Given
        var logger = Substitute.For<ILogger<UpdateActivityDatesUseCase>>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var sut = (IUpdateActivityDatesUseCase)new UpdateActivityDatesUseCase(logger, unitOfWork);

        var workerA = new Worker(Guid.NewGuid(), "A");
        var workerB = new Worker(Guid.NewGuid(), "B");
        var activityA = new BuildMachineActivity(Guid.NewGuid(), new DateTime(2022, 10, 20, 10, 10, 00), new DateTime(2022, 10, 20, 10, 11, 00));

        var toBeUpdatedActivity = new BuildMachineActivity(Guid.NewGuid(), activityA.FinishRestingDate, activityA.FinishRestingDate.AddMinutes(1));

        workerA.WorksIn(activityA).WorksIn(toBeUpdatedActivity);
        workerB.WorksIn(activityA).WorksIn(toBeUpdatedActivity);
        
        unitOfWork.WorkerRepository.GetWorkersAsync(default).ReturnsForAnyArgs(new[] { workerA, workerB });
        unitOfWork.ActivityRepository.GetAsync(Arg.Is(toBeUpdatedActivity.Id)).Returns(toBeUpdatedActivity);

        var command = new UpdateActivityDatesCommand(
            activity: toBeUpdatedActivity.Id, 
            newStartDate: new DateTime(2022, 10, 11), 
            newEndDate: new DateTime(2022, 10, 20, 10, 15, 00));
    
        // When
        var result = await sut.HandleAsync(command);

        // Then
        result.IsSuccess.Should().BeTrue();
        workerA.Activities.First().Start.Should().Be(command.NewStartDate);
        workerB.Activities.First().Start.Should().Be(command.NewStartDate);
        toBeUpdatedActivity.Start.Should().Be(command.NewStartDate);
    }

    [Fact]
    public async void Updating_An_Component_Activity_Date_For_More_Than_One_Worker_Should_Be_Fail()
    {
        // Given
        var logger = Substitute.For<ILogger<UpdateActivityDatesUseCase>>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        var sut = (IUpdateActivityDatesUseCase)new UpdateActivityDatesUseCase(logger, unitOfWork);

        var workerA = new Worker(Guid.NewGuid(), "A");
        var workerB = new Worker(Guid.NewGuid(), "B");
        var activityA = new BuildMachineActivity(Guid.NewGuid(), new DateTime(2022, 10, 20, 10, 10, 00), new DateTime(2022, 10, 20, 10, 11, 00));

        var toBeUpdatedActivity = new BuildComponentActivity(Guid.NewGuid(), activityA.FinishRestingDate, activityA.FinishRestingDate.AddMinutes(1));

        workerA.WorksIn(activityA).WorksIn(toBeUpdatedActivity);
        workerB.WorksIn(activityA).WorksIn(toBeUpdatedActivity);

        unitOfWork.WorkerRepository.GetWorkersAsync(Arg.Is(toBeUpdatedActivity.Workers)).Returns(new[] { workerA, workerB });
        unitOfWork.ActivityRepository.GetAsync(Arg.Is(toBeUpdatedActivity.Id)).Returns(toBeUpdatedActivity);

        var command = new UpdateActivityDatesCommand(
            activity: toBeUpdatedActivity.Id,
            newStartDate: new DateTime(2022, 10, 11),
            newEndDate: new DateTime(2022, 10, 20, 10, 15, 00));

        // When
        var result = await sut.HandleAsync(command);

        // Then
        result.IsSuccess.Should().BeTrue();
        workerA.Activities.First().Start.Should().Be(command.NewStartDate);
        workerB.Activities.First().Start.Should().Be(command.NewStartDate);
        toBeUpdatedActivity.Start.Should().Be(command.NewStartDate);
    }
}