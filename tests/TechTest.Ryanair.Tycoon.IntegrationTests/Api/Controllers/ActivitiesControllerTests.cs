using Flurl.Http;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using TechTest.Ryanair.Tycoon.Api.Requests;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;
using TechTest.Ryanair.Tycoon.IntegrationTests.Fixtures;

namespace TechTest.Ryanair.Tycoon.IntegrationTests.Api.Controllers;

public class ActivitiesControllerTests
{
    private readonly TestServer _testServer;
    private readonly IFlurlClient _client;

    public ActivitiesControllerTests()
    {
        var fixture = new TestServerFixture();
        _testServer = fixture.Server;
        _client = fixture.Client;
    }

    [Fact]
    public async Task Scheduling_Activity_For_Existing_Workers_Should_Be_Ok()
    {
        var request = new CreateWorkerRequest() { Name = "A" };
        var response = await _client.Request("/api/workers").AllowAnyHttpStatus().PostJsonAsync(request).ReceiveJson();
        var id = response.id;

        var schedule = new ScheduleActivityRequest()
        {
            Id = Guid.NewGuid(),
            StartDate = new DateTime(2022, 08, 08, 10, 10, 00),
            FinishDate = new DateTime(2022, 08, 08, 15, 00, 00),
            Workers = new List<string>() { id },
            Type = "Component"
        };

        var actual = await _client.Request("/api/activities/schedule")
            .AllowAnyHttpStatus()
            .PostJsonAsync(schedule);
        var content = await actual.GetJsonAsync<ScheduledActivityResponse>();

        actual.StatusCode.Should().Be((int)HttpStatusCode.Created);
        actual.Headers.Should().Contain(header => header.Name == "Location" && header.Value.Contains(request.Id.ToString()));
        content.ActivityId.Should().Be((Guid)schedule.Id);
    }

    [Theory]
    [InlineData("Component")]
    [InlineData("Machine")]
    public async Task Creating_Standalone_Activity_Should_Work(string activityType)
    {
        // Given
        var request = new CreateActivityRequest()
        {
            ActivityType = activityType,
            Id = Guid.NewGuid(),
            StartDate = new DateTime(2022, 08, 02),
            FinishDate = new DateTime(2022, 08, 04)
        };

        // When
        var actual = await _client.Request("/api/activities")
            .AllowAnyHttpStatus()
            .PostJsonAsync(request);

        var content = await actual.GetJsonAsync<CreatedActivityResponse>();

        // Then
        actual.StatusCode.Should().Be((int)HttpStatusCode.Created);
        actual.Headers.Should().Contain(header => header.Name == "Location" && header.Value.Contains(request.Id.ToString()));

        content.FinishDate.Should().Be(request.FinishDate);
        content.StartDate.Should().Be(request.StartDate);
        content.Id.Should().Be(request.Id);
    }
}
