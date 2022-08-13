using Flurl.Http;
using System.Net;
using TechTest.Ryanair.Tycoon.Api.Requests.Workers;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.IntegrationTests.Fixtures;

namespace TechTest.Ryanair.Tycoon.IntegrationTests.Api.Controllers
{
    public class WorkersControllerTests
    {
        private readonly TestServerFixture _fixture;
        public WorkersControllerTests() => _fixture = new TestServerFixture();

        [Fact]
        public async Task Getting_Registered_Worker_By_Id_Should_Work()
        {
            // Given
            var request = new CreateWorkerRequest() { Name = "A" };

            var created = await _fixture.Client
                .Request("/api/workers")
                .PostJsonAsync(request)
                .ReceiveJson<CreatedWorkerResponse>();

            var id = created.Id;

            // When 
            var response = await _fixture.Client
                .Request($"/api/workers/{id}")
                .WithHeader("Content-Type", "application/json")
                .AllowAnyHttpStatus()
                .GetAsync();

            var actual = await response.GetJsonAsync();

            response.StatusCode.Should().Be((int)HttpStatusCode.OK);

            // dynamic objects doesn't work with fluent assertions or typed assertions.
            Assert.True(id.ToString().Equals(actual.id.ToString()));
            Assert.True(request.Name.Equals(actual.name));
            Assert.True(Worker.Status.Idle.ToString().Equals(actual.status.ToString()));
        }

        [Fact]
        public async Task Getting_Inexistent_Worker_Should_Fail()
        {
            // Given
            var inexistentId = Guid.NewGuid();

            // When 
            var response = await _fixture.Client
                .Request($"/api/workers/{inexistentId}")
                .AllowAnyHttpStatus()
                .GetAsync();

            var actual = await response.GetJsonAsync();

            // Then
            response.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
            Assert.Equal(ApplicationErrors.WorkerNotFound.Code, actual.code.ToString());
            Assert.Equal(ApplicationErrors.WorkerNotFound.Message, actual.message.ToString());
        }
    }
}
