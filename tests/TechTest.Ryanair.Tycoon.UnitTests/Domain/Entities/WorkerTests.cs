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
    public void Null_Worker_Should_Not_Work_In_Activities()
    {
        var sut = Worker.Null;

        var result = sut.WorksIn(new BuildMachineActivity(Guid.NewGuid(), DateTime.Today, DateTime.Now.AddHours(1)));

        result.IsFailed.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.AddingActivityToNullWorker);
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
