using Xunit;
using TechTest.Ryanair.Tycoon.Domain.Entities; 

namespace TechTest.Ryanair.Tycoon.UnitTests.Domain.Entities;

public class WorkerTests
{
    [Fact]
    public void Adding_An_Activity_Should_Validate_Overlaps()
    {
        // Given
        var sut = new Worker(name: "A", id: Guid.NewGuid());
        
        // When
        var activiy = new ComponentBuildActivity(startDate: DateTime.UtcNow, endDate: DateTime.Now.AddDays(1));
        var overlapping = new MachineBuildActivity(startDate: DateTime.Now.AddMinutes(30), endDate: DateTime.Now.AddHours(40));
    
        // Then
        var result = 
            sut.WorksIn(activiy)
                .WorksIn(overlapping);

        result.IsSucces.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.OverlappingActivities);
        result.OverlappingActivities.Should().HaveCount(1);
    }
    
    [Fact]
    public void Duplicated_Activities_Should_Fail()
    {
        // Given
        var sut = new Worker(name: "A", id: Guid.NewGuid());
        
        // When
        var activiy = new ComponentBuildActivity(startDate: DateTime.UtcNow, endDate: DateTime.Now.AddDays(1));
        var overlapping = new MachineBuildActivity(startDate: DateTime.Now.AddMinutes(30), endDate: DateTime.Now.AddHours(40));
    
        // Then
        var result = 
            sut.WorksIn(activiy)
                .WorksIn(overlapping);

        result.IsSucces.Should().BeFalse();
        result.Error.Should().Be(DomainErrors.OverlappingActivities);
        result.OverlappingActivities.Should().HaveCount(1);
    }
}