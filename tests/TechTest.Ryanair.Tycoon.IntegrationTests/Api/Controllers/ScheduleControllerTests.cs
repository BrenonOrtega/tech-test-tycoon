using Flurl.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using TechTest.Ryanair.Tycoon.Api.Requests;

namespace TechTest.Ryanair.Tycoon.IntegrationTests.Api.Controllers;

public class ScheduleControllerTests
{
    private readonly TestServer _testServer;
    private readonly FlurlClient _client;

    public ScheduleControllerTests()
    {
        _testServer = new WebApplicationFactory<Program>().Server;
        _client = new FlurlClient(_testServer.CreateClient());
    }
    [Fact]
    public async Task Scheduling_Activity_For_Existing_Workers_Should_Be_Ok()
    {
        var request = new CreateWorkerRequest() { Name = "A" };
        var response = await _client.Request("/api/workers").AllowAnyHttpStatus().PostJsonAsync(request).ReceiveJson();

        var schedule = new ScheduleActivityRequest()
        {
            StartDate = new DateTime(2022, 08, 08, 10, 10, 00),
            FinishDate = new DateTime(2022, 08, 08, 15, 00, 00),
            Workers = new List<string>() { "A" }
        };

        var actual = await _client.Request("/api/schedule")
            .PostJsonAsync(schedule);

        actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
}
