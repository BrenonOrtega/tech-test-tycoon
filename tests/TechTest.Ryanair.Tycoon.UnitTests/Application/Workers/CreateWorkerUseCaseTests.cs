using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Application;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.UnitTests.Application.Workers;

public class CreateWorkerUseCaseTests
{
    [Fact]
    public async Task Creating_Valid_Worker_Should_Pass()
    {
        var logger = Substitute.For<ILogger<CreateWorkerUseCase>>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        var request = new CreateWorkerCommand(Guid.NewGuid(), "A");
        ICreateWorkerUseCase sut = new CreateWorkerUseCase(logger, unitOfWork);

        var result = await sut.HandleAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(request.Id);
        await unitOfWork.Received().WorkerRepository.CreateAsync(Arg.Is<Worker>(x => x.Id == request.Id && x.Name == request.Name));
    }

    [Theory]
    [MemberData(nameof(InvalidWorkerGenerator))]
    public async Task Invalid_Worker_Should_Fail(CreateWorkerCommand invalidCommand, Error expectedError)
    {
        var logger = Substitute.For<ILogger<CreateWorkerUseCase>>();
        var unitOfWork = Substitute.For<IUnitOfWork>();

        ICreateWorkerUseCase sut = new CreateWorkerUseCase(logger, unitOfWork);

        var result = await sut.HandleAsync(invalidCommand);

        result.IsFailed.Should().BeTrue();
        result.Error.Should().Be(expectedError);
    }

    public static IEnumerable<object[]> InvalidWorkerGenerator()
    {
        yield return new object[] { null, ApplicationErrors.NullCommand };
        yield return new object[] { new CreateWorkerCommand(Guid.Empty, "A"), ApplicationErrors.InvalidCommand };
        yield return new object[] { new CreateWorkerCommand(Guid.NewGuid(), ""), ApplicationErrors.InvalidCommand };
        yield return new object[] { new CreateWorkerCommand(Guid.NewGuid(), null), ApplicationErrors.InvalidCommand };
    }
}
