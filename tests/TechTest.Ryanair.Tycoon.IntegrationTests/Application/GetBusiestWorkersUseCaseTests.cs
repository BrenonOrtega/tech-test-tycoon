using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechTest.Ryanair.Tycoon.Application.Extensions;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases;
using TechTest.Ryanair.Tycoon.Domain.Repositories;
using TechTest.Ryanair.Tycoon.Infra.Extensions;
using TechTest.Ryanair.Tycoon.UnitTests.Application.Workers;

namespace TechTest.Ryanair.Tycoon.IntegrationTests.Application;

public class GetBusiestWorkersUseCaseTests
{

    [Fact]
    public async Task Getting_Busiest_Workers_Should_Pass()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>()
            {
                { $"{nameof(WorkerUseCaseOptions)}:GetBusiestQuantityDefault", "10" }
            })
            .Build();

        var provider = new ServiceCollection()
            .AddInfrastructure()
            .AddUseCases(configuration)
            .AddLogging()
            .BuildServiceProvider();

        var repo = provider.GetRequiredService<IWorkerRepository>();
        var sut = (IGetBusiestWorkersUseCase)new GetBusiestWorkersUseCase(repo);

        var result = await sut.HandleAsync(new GetBusiestWorkersCommand() { Count = 10 }); ;
    }
}
