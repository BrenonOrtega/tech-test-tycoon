using Flurl.Http;
using System.Net;
using TechTest.Ryanair.Tycoon.Api.Requests;
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
                .AllowAnyHttpStatus()
                .PostJsonAsync(request)
                .ReceiveJson<CreatedWorkerResponse>();

            var id = created.Id;

            // When 
            var response = await _fixture.Client
                .Request($"/Api/workers/{id}")
                .GetAsync();

            var actual = await response.GetJsonAsync<Worker>();
            response.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Should().NotBeNull();
            actual.Should().NotBe(Worker.Null);
            actual.Name.Should().Be(request.Name);
        }
    }
}
