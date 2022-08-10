using Awarean.Sdk.Result;
using Microsoft.Extensions.DependencyInjection;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.AssignExistentActivity;
using TechTest.Ryanair.Tycoon.Application.Extensions;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.Infra.Extensions;

namespace TechTest.Ryanair.Tycoon.IntegrationTests.Application
{
    public class AssignExistentActivityUseCaseTests
    {
        [Fact]
        public async Task Assigning_Existent_Activity_ToValid_Workers_Should_Be_Success()
        {
            var provider = new ServiceCollection().AddUseCases().AddInfrastructure().AddLogging().BuildServiceProvider();
            var ids = await SeedWorkers(provider, 3);
            var sut = provider.GetRequiredService<IAssignExistentActivityUseCase>();

            var command = new AssignExistentActivityCommand(activityId: Guid.NewGuid(), workers: ids);

            Result<AssignedActivityResponse> result = await sut.HandleAsync(command);
            result.IsSuccess.Should().BeTrue();
        }

        private async Task<IEnumerable<Guid>> SeedWorkers(ServiceProvider provider, int count)
        {
            var guids = new List<Guid>();
            for (var i = 0; i < count; i++)
            {
                var command = new CreateWorkerCommand(Guid.NewGuid(), i.ToString());
                await provider.GetRequiredService<ICreateWorkerUseCase>().HandleAsync(command);
                guids.Add(command.Id);
            }

            return guids;
        }
    }
}
