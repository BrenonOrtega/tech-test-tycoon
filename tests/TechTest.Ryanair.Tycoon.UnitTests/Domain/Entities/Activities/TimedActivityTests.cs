using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.UnitTests.Domain.Entities.Activities
{
    public class TimedActivityTests
    {
        [Theory]
        [MemberData(nameof(OverlappingActivitiesGenerator))]
        public void When_Checking_Activities_Should_Say_If_Overlaps(TimedActivity first, TimedActivity second)
        {
            var overlaps = first.Overlaps(second);

            overlaps.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(NonOverlappingActivitiesGenerator))]
        public void No_Overlaps_Should_Succeed_Validation(TimedActivity first, TimedActivity second)
        {
            var overlaps = first.Overlaps(second);

            overlaps.Should().BeFalse();
        }

        [Fact]
        public void Start_Date_Cannot_Be_After_Finish_Date()
        {
            var wrongAction = () => new BuildComponentActivity(Guid.NewGuid(), DateTime.Now, DateTime.Now.AddMilliseconds(-1));
            wrongAction.Should().Throw<ArgumentException>();
        }

        [Theory]
        [MemberData(nameof(TypeActivitiesGenerator))]
        public void Activity_Should_Tell_Its_Resting_Time(Type type)
        {
            var activityType = Activator.CreateInstance(type, new object[] { Guid.NewGuid(), DateTime.Now, DateTime.Now.AddSeconds(1)}) as TimedActivity;

            activityType.FinishRestingDate.Should().BeMoreThan(TimeSpan.Zero);
        }

        [Theory]
        [MemberData(nameof(TypeActivitiesGenerator))]
        public void Activity_Should_Tell_Its_Type(Type type)
        {
            var activityType = Activator.CreateInstance(type, new object[] { Guid.NewGuid(), DateTime.Now, DateTime.Now.AddSeconds(1) }) as TimedActivity;

            activityType.Type.Should().NotBe(String.Empty);
            activityType.Type.Should().NotBeNull();
        }

        public static IEnumerable<object[]> TypeActivitiesGenerator() 
            => typeof(TimedActivity).Assembly
                .GetTypes()
                .Where(x => x.IsSubclassOf(typeof(TimedActivity)))
                .Select(x => new object[] { x });

        public static IEnumerable<object[]> OverlappingActivitiesGenerator()
        {
            yield return new object[]
            {   //Second Activity Overlaps by seconds
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:33, second:20), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:50, second:50)),
                new BuildMachineActivity(   Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:50, second:20), new DateTime(year: 2022, month: 05, day: 10, hour: 16, minute:35, second:00))
            };

            yield return new object[]
            {   //First Activity Overlaps by hours
                new BuildMachineActivity(   Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 12, minute:0, second:0), new DateTime(year: 2022, month: 05, day: 10, hour: 17, minute:0, second:00)),
                new BuildMachineActivity(   Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 13, minute:0, second:0), new DateTime(year: 2022, month: 05, day: 10, hour: 13, minute:30, second:00))
            };

            yield return new object[]
            {   //Activity overlaps during first by days
                new BuildMachineActivity(   Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:33, second:20), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:50, second:50)),
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 09, hour: 00, minute:00, second:00), new DateTime(year: 2022, month: 05, day: 10, hour: 16, minute:35, second:00))
            };

            yield return new object[]
            {   // Second overlaping by an entire day
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:33, second:20), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:50, second:50)),
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 09, hour: 15, minute:50, second:20), new DateTime(year: 2022, month: 05, day: 11, hour: 16, minute:35, second:00))
            };

            yield return new object[]
            {   // Second overlaping by rest time by the last second.
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 14, minute:33, second:20), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:00, second:00)),
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 16, minute:59, second:59), new DateTime(year: 2022, month: 05, day: 11, hour: 18, minute:00, second:00))
            };
        }

        public static IEnumerable<object[]> NonOverlappingActivitiesGenerator()
        {
            yield return new object[]
            {
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 20, hour: 10, minute:00, second:00), new DateTime(year: 2022, month: 05, day: 20, hour: 15, minute:00, second:00)),
                new BuildMachineActivity(   Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 20, hour: 17, minute:00, second:00), new DateTime(year: 2022, month: 05, day: 20, hour: 18, minute:35, second:00))
            };

            yield return new object[]
            {
                new BuildMachineActivity(   Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 12, minute:0, second:0), new DateTime(year: 2022, month: 05, day: 10, hour: 17, minute:0, second:00)),
                new BuildMachineActivity(   Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 21, minute:0, second:0), new DateTime(year: 2022, month: 05, day: 10, hour: 21, minute:30, second:00))
            };

            yield return new object[]
            {
                new BuildMachineActivity(   Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 09, hour: 00, minute:00, second:00), new DateTime(year: 2022, month: 05, day: 10, hour: 00, minute:00, second:00)),
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 4, minute:00, second:00), new DateTime(year: 2022, month: 05, day: 10, hour: 16, minute:00, second:00))
            };

            yield return new object[]
            {
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:33, second:20), new DateTime(year: 2022, month: 05, day: 10, hour: 15, minute:50, second:50)),
                new BuildComponentActivity( Guid.NewGuid(), new DateTime(year: 2022, month: 05, day: 05, hour: 15, minute:50, second:20), new DateTime(year: 2022, month: 05, day: 06, hour: 16, minute:35, second:00))
            };
        }
    }
}