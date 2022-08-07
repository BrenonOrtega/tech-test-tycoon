using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application.CreateWorker;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.UnitTests.Application
{
    public class CreateWorkerUseCaseTests
    {
        [Fact]
        public async Task Creating_Valid_Use_Should_Pass()
        {
            var logger = Substitute.For<ILogger<CreateWorkerUseCase>>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            var request = new CreateWorkerCommand() { Id = Guid.NewGuid(), Name = "A" };
            ICreateWorkerUseCase sut = new CreateWorkerUseCase(logger, unitOfWork);

            var result = await sut.HandleAsync(request);

            result.IsSuccess.Should().BeTrue();
            result.Value.Id.Should().Be(request.Id);
        }
    }
}
