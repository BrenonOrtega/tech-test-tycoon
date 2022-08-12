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

        var provider = new ServiceCollection()
            .AddInfrastructure()
            .AddUseCases(configuration)
            .AddLogging()
            .BuildServiceProvider();

        var repo = provider.GetRequiredService<IWorkerRepository>();
        var options = provider.GetRequiredService<IOptionsSnapshot<GetBusiestWorkersOptions>>();
        var sut = (IGetBusiestWorkersUseCase)new GetBusiestWorkersUseCase(repo, options);

        var result = await sut.HandleAsync(new GetBusiestWorkersCommand(10, new DateTime(2022, 10, 10), new DateTime(2022, 10, 11)));

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(Enumerable.Empty<Worker>());
    }
}
