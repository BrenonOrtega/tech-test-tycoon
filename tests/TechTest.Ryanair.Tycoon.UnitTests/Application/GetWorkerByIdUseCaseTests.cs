using TechTest.Ryanair.Tycoon.Domain.Repositories;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Infra.Repositories;
using Microsoft.Extensions.Logging;

namespace TechTest.Ryanair.Tycoon.UnitTests.Application;

public class GetWorkerByIdUseCaseTests
{
    [Fact]
    public async Task Getting_Existent_Worker_By_Id_Should_Find_Succesfully()
    {
        var command = new GetWorkerByIdCommand(id: Guid.NewGuid());
        var expected = new Worker(command.Id, "A");
        var data = new Dictionary<Guid, Worker>() { { expected.Id, expected } };
        var logger = Substitute.For<ILogger<WorkerInMemoryRepository>>();
        var repo = (IWorkerRepository)new WorkerInMemoryRepository(data, logger);

        var sut = (IGetWorkerByIdUseCase)new GetWorkerByIdUseCase(repo);


        var result = await sut.HandleAsync(command);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(expected);
    }
}
