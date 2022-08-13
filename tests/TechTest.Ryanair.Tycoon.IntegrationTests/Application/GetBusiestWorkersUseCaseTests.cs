using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TechTest.Ryanair.Tycoon.Application.Extensions;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;
using TechTest.Ryanair.Tycoon.Infra.Extensions;

namespace TechTest.Ryanair.Tycoon.IntegrationTests.Application;

public class GetBusiestWorkersUseCaseTests
{
    [Fact]
    public async Task Getting_Busiest_Workers_Should_Pass()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>()
            {
                { $"{nameof(GetBusiestWorkersOptions)}:{nameof(GetBusiestWorkersOptions.GetQuantity)}", "10" }
            })
            .Build();

        var provider = new ServiceCollection().AddInfrastructure().AddUseCases(configuration).AddLogging().BuildServiceProvider();

        var repo = provider.GetRequiredService<IWorkerRepository>();
        var options = provider.GetRequiredService<IOptionsSnapshot<GetBusiestWorkersOptions>>();
        var sut = (IGetBusiestWorkersUseCase)new GetBusiestWorkersUseCase(repo, options);

        var workers = await SeedDatabaseAsync(repo);

        var result = await sut.HandleAsync(new GetBusiestWorkersCommand(count: 2, new DateTime(2019, 1, 1), new DateTime(2022, 10, 11)));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(workers.Take(2));
    }

    private async Task<List<Worker>> SeedDatabaseAsync(IWorkerRepository repo)
    {
        var workers = new List<Worker>()
        {
            new Worker(Guid.NewGuid(), "A")
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 1), new DateTime(2019, 1, 2)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 3), new DateTime(2019, 1, 4)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 5), new DateTime(2019, 1, 6)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 7), new DateTime(2019, 1, 8)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 9), new DateTime(2019, 1, 10)))
                .Value,
            new Worker(Guid.NewGuid(), "B")
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 1), new DateTime(2019, 1, 2)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 3), new DateTime(2019, 1, 4)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 5), new DateTime(2019, 1, 6)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 7), new DateTime(2019, 1, 8)))
                .Value,
            new Worker(Guid.NewGuid(), "C")
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 1), new DateTime(2019, 1, 2)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 3), new DateTime(2019, 1, 4)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 5), new DateTime(2019, 1, 6)))
                .Value,
            new Worker(Guid.NewGuid(), "D")
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 1), new DateTime(2019, 1, 2)))
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 3), new DateTime(2019, 1, 4)))
                .Value,
            new Worker(Guid.NewGuid(), "E")
                .WorksIn(new BuildComponentActivity(Guid.NewGuid(), new DateTime(2019, 1, 1), new DateTime(2019, 1, 2)))
                .Value,
            new Worker(Guid.NewGuid(), "F"),
        };
        
        foreach(var worker in workers)
            await repo.CreateAsync(worker);

        return workers;
    }
}
