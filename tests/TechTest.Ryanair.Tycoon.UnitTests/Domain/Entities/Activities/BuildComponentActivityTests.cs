using TechTest.Ryanair.Tycoon.Domain;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.UnitTests.Domain.Entities.Activities
{
    public class BuildComponentActivityTests
    {

        [Fact]
        public void Adding_More_Than_One_Worker_To_Component_Activity_Should_Fail()
        {
            var workers = new List<Worker>
            {
                new(Guid.NewGuid(), "A"),
                new(Guid.NewGuid(), "B"),
            };

            var sut = new BuildComponentActivity(Guid.NewGuid(), DateTime.Now.AddHours(-1), DateTime.Now.AddMinutes(30));

            var first = workers[0].WorksIn(sut);

            var second = workers[1].WorksIn(sut);

            first.IsSuccess.Should().BeTrue();
            second.IsSuccess.Should().BeFalse();
            second.Error.Should().Be(DomainErrors.InvalidActivityAssignment);
        }
    }
}
