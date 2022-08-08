using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using TechTest.Ryanair.Tycoon.Api.Requests;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.IntegrationTests.Fixtures;

namespace TechTest.Ryanair.Tycoon.IntegrationTests.Api.Controllers;

public class ScheduleControllerTests
{
    private readonly TestServer _testServer;
    private readonly IFlurlClient _client;

    public ScheduleControllerTests()
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

        var actual = await _client.Request("/api/schedule")
            .AllowAnyHttpStatus()
            .PostJsonAsync(schedule);

        var content = await actual.GetJsonAsync<ScheduledActivityResponse>();

        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
        content.ActivityId.Should().Be((Guid)schedule.Id);
    }
}
