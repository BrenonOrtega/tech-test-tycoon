using Microsoft.Extensions.DependencyInjection;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;
using TechTest.Ryanair.Tycoon.Infra;
using TechTest.Ryanair.Tycoon.Infra.Extensions;

namespace TechTest.Ryanair.Tycoon.IntegrationTests.Infra.Repositories;

public class WorkerRepositoryTests
{
    private readonly IWorkerRepository _sut;
    private readonly Dictionary<Guid, Worker> _data;

    public WorkerRepositoryTests()
    {
        var provider = new ServiceCollection().AddLogging().AddInfrastructure().BuildServiceProvider();

        _sut = provider.GetRequiredService<IWorkerRepository>();
        _data = provider.GetRequiredService<Dictionary<Guid, Worker>>();
    }

    [Fact]
    public async Task Adding_New_Worker_Should_Add_Correctly()
    {
        var worker = new Worker(Guid.NewGuid(), "A");

        var result = await _sut.CreateAsync(worker);

        result.IsSuccess.Should().BeTrue();
        _data.Should().Contain(KeyValuePair.Create(worker.Id, worker));
    }

    [Fact]
    public async Task Updating_Worker_Should_Work()
    {
        var worker = new Worker(Guid.NewGuid(), "A");

        await _sut.CreateAsync(worker);

        var update = new Worker(worker.Id, worker.Name)
            .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2022, 08, 08), new DateTime(2022, 08, 09)))
            .Value;

        await _sut.UpdateAsync(worker.Id, update);

        var updated = await _sut.GetAsync(worker.Id);

        updated.Should().Be(update);
        updated.Activities.Should().BeEquivalentTo(update.Activities);
    }

    [Fact]
    public async Task Invalid_Worker_Should_Not_Save()
    {
        var result = await _sut.CreateAsync(null);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(InfrastructureErrors.NullEntity);
        _data.Should().BeEmpty();
    }

    [Fact]
    public async Task Invalid_Worker_Should_Not_Update()
    {
        var worker = new Worker(Guid.NewGuid(), "A");
        await _sut.CreateAsync(worker);

        var result = await _sut.UpdateAsync(worker.Id, null);

        var actual = await _sut.GetAsync(worker.Id);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(InfrastructureErrors.InvalidUpdateCommand);
        actual.Should().Be(worker);
    }

    ~WorkerRepositoryTests()
    {
        _data.Clear();
    }
}