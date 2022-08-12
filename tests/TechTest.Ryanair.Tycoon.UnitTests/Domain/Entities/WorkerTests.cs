using TechTest.Ryanair.Tycoon.Domain;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.UnitTests.Domain.Entities;

public class WorkerTests
{
    [Theory]
    [MemberData(nameof(InvalidActivityGenerator))]
    public void Invalid_Activities_Should_Be_Invalid(TimedActivity invalidActivity)
    {
        // Given
        var sut = new Worker(name: "A", id: Guid.NewGuid());

        // When
        var result = sut.WorksIn(invalidActivity);

        // Then
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.TryWorkingInInvalidActivity);
        result.FailedActivity.Should().Be(TimedActivity.Null);
    }

    [Fact]
    public void Adding_An_Activity_Should_Validate_Overlaps()
    {
        // Given
        var sut = new Worker(name: "A", id: Guid.NewGuid());

        // When
        var activity = new BuildComponentActivity(id: Guid.NewGuid(), start: DateTime.Now, finish: DateTime.Now.AddDays(1));
        var overlapping = new BuildMachineActivity(id: Guid.NewGuid(), start: DateTime.Now.AddMinutes(-30), finish: DateTime.Now.AddHours(40));

        // Then
        var result =
            sut.WorksIn(activity)
                .WorksIn(overlapping);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.OverlappingActivities);
        result.FailedActivity.Should().Be(overlapping);
    }

    [Fact]
    public void Duplicated_Activities_Should_Fail()
    {
        // Given
        var sut = new Worker(name: "A", id: Guid.NewGuid());

        // When
        var activity = new BuildComponentActivity(id: Guid.NewGuid(), start: DateTime.UtcNow, finish: DateTime.Now.AddDays(1));

        // Then
        var result =
            sut.WorksIn(activity)
                .WorksIn(activity);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.AlreadyWorksInActivity);
        result.FailedActivity.Should().Be(activity);
    }

    [Fact]
    public void Workers_Should_Remove_Activities_When_Asked()
    {
        // Given
        var sut = new Worker(name: "A", id: Guid.NewGuid());

        var activity = new BuildComponentActivity(id: Guid.NewGuid(), start: DateTime.UtcNow, finish: DateTime.Now.AddDays(1));
        sut.WorksIn(activity);

        // When
        var result = sut.Unassign(activity);

        // Then
        result.IsSuccess.Should().BeTrue();
        sut.Activities.Should().NotContain(activity);
        activity.Workers.Should().NotContain(sut.Id);
    }

    [Fact]
    public void Removing_NotAssined_Activities_Should_Fail()
    {
        // Given
        var sut = new Worker(name: "A", id: Guid.NewGuid());

        // When
        var activity = new BuildComponentActivity(id: Guid.NewGuid(), start: DateTime.UtcNow, finish: DateTime.Now.AddDays(1));

        // Then
        var result = sut.Unassign(activity);

        result.IsFailed.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.ActivityNotAssignedToWorker);
    }

    [Fact]
    public void Null_Worker_Should_Not_Work_In_Activities()
    {
        var sut = Worker.Null;

        var result = sut.WorksIn(new BuildMachineActivity(Guid.NewGuid(), DateTime.Today, DateTime.Now.AddHours(1)));

        result.IsFailed.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.AddingActivityToNullWorker);
    }

    [Fact]
    public void Assign_Activities_During_Recharge_Time_Are_NotAllowed()
    {
        var sut = new Worker(name: "A", id: Guid.NewGuid());

        sut.WorksIn(new BuildMachineActivity(Guid.NewGuid(), DateTime.Now, DateTime.Now.AddMinutes(1)));

        var result = sut.WorksIn(new BuildMachineActivity(Guid.NewGuid(), DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(10)));

        result.IsFailed.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.ActivityScheduledInRestTime);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void Worker_WorkPeriod_Filtered_By_Time_Period_Should_Work(int count)
    {
        var sut = new Worker(name: "A", id: Guid.NewGuid());

        var initialDateTime = new DateTime(2022, 8, 12, 1, 0, 0);
        var startDate = initialDateTime;
        var finalDateTime = startDate;

        for (var i=0; i <= count; i++)
        {
            var activity = OneHourActivityGenerator(startDate);
            startDate = activity.FinishRestingDate;
            sut.WorksIn(activity);
            finalDateTime = activity.Finish;
        }

        var actualWorkTime = sut.WorkTimeBetween(initialDateTime, finalDateTime);

        var expected = sut.Activities.Select(x => x.Duration).Aggregate(TimeSpan.Zero, (initial, next) => initial + next);
        actualWorkTime.Should().Be(expected);

        static TimedActivity OneHourActivityGenerator(DateTime startDate)
            => new BuildMachineActivity(Guid.NewGuid(), startDate.AddSeconds(1), startDate.AddHours(1));
    }

    [Fact]
    public void Getting_Work_Time_Between_Should_Only_Return_Period_Between_Time_Range()
    {
        var sut = new Worker(name: "A", id: Guid.NewGuid());

        var initialDateTime = new DateTime(2022, 8, 12);
        var finalDateTime = new DateTime(2022, 8, 12, 15, 0, 0);

        var activity = new BuildComponentActivity(Guid.NewGuid(), initialDateTime.AddHours(-10), initialDateTime.AddHours(2));
        var activityB = new BuildComponentActivity(Guid.NewGuid(), activity.FinishRestingDate, finalDateTime.AddHours(3));

        var restTime = activity.RestPeriod;

        sut.WorksIn(activity).WorksIn(activityB);

        var actualWorkTime = sut.WorkTimeBetween(initialDateTime, finalDateTime);

        var expected = finalDateTime - initialDateTime - restTime;

        actualWorkTime.Should().Be(expected);
    }

    [Theory]
    [MemberData(nameof(StatusGenerator))]
    public void Worker_Status_Should_Depend_On_Activities(Worker.Status expectedStatus, TimedActivity activity)
    {
        var worker = new Worker(Guid.NewGuid(), "A");

        var result = worker.WorksIn(activity);

        result.IsSuccess.Should().BeTrue();

        var actual = (Worker)result.Value;

        actual.ActualStatus.Should().Be(expectedStatus);
    }

    public static IEnumerable<object[]> StatusGenerator()
    {
        yield return new object[] { Worker.Status.Working, new BuildComponentActivity(Guid.NewGuid(), DateTime.Now.AddSeconds(-1), DateTime.Now.AddSeconds(30)) };
        yield return new object[] { Worker.Status.Recharging, new BuildComponentActivity(Guid.NewGuid(), DateTime.Now.AddSeconds(-10), DateTime.Now.AddSeconds(-1)) };
        yield return new object[] { Worker.Status.Idle, new BuildComponentActivity(Guid.NewGuid(), DateTime.Now.AddDays(-1), DateTime.Now.AddHours(-5)) };
    }

    public static IEnumerable<object[]> InvalidActivityGenerator()
    {
        yield return new object[] { null };
        yield return new object[] { TimedActivity.Null };
    }
}
