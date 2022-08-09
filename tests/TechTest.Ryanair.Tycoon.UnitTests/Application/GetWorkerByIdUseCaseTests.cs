using TechTest.Ryanair.Tycoon.Domain.Repositories;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Infra.Repositories;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application;

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
        result.Value.Should().BeEquivalentTo(expected, 
            config => config.WithMapping<FoundWorkerResponse>(c => c.ActualStatus, e => e.Status));
    }

    [Theory]
    [MemberData(nameof(InvalidCommandGenerator))]
    public async Task Invalid_Command_Should_Fail(GetWorkerByIdCommand invalidCommand)
    {
        // Given
        var repo = Substitute.For<IWorkerRepository>();
        var sut = (IGetWorkerByIdUseCase)new GetWorkerByIdUseCase(repo);

        // When
        var result = await sut.HandleAsync(invalidCommand);

        // Then
        var expectedErrors = new[] { ApplicationErrors.InvalidCommand, ApplicationErrors.InvalidGuid, ApplicationErrors.NullCommand };

        result.IsFailed.Should().BeTrue();
        expectedErrors.Should().Contain(result.Error);
    }

    [Fact]
    public async Task NotFound_Worker_Should_Fail_Request()
    {
        // Given
        var repo = Substitute.For<IWorkerRepository>();
        var sut = (IGetWorkerByIdUseCase)new GetWorkerByIdUseCase(repo);
        var request = new GetWorkerByIdCommand(Guid.NewGuid());
        repo.GetAsync(Arg.Any<Guid>()).Returns(Worker.Null);

        // When
        var result = await sut.HandleAsync(request);

        // Then
        result.IsFailed.Should().BeTrue();
        result.Error.Should().Be(ApplicationErrors.WorkerNotFound);
    }

    public static IEnumerable<object[]> InvalidCommandGenerator()
    {
        yield return new object[] { null };
        yield return new object[] { new GetWorkerByIdCommand(Guid.Empty) };
    }
}
