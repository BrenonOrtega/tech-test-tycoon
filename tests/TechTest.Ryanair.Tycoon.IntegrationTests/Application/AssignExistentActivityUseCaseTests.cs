using Awarean.Sdk.Result;
using Microsoft.Extensions.DependencyInjection;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity.AssignExistent;
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

            var create = new CreateActivityCommand(Guid.NewGuid(), "Machine", new DateTime(2022, 03, 10), new DateTime(2022, 03, 11));
                
            await provider.GetRequiredService<ICreateActivityUseCase>().HandleAsync(create);

            var command = new AssignExistentActivityCommand(activityId: create.Id, ids);

            // ERROR IS THAT IT CREATES, SHOULD BE AN UPDATE - FIXED, NOW IT WILL BREAK TESTS.
            Result<AssignedActivityResponse> result = await sut.HandleAsync(command);
            result.IsSuccess.Should().BeTrue();
        }

        private async Task<IEnumerable<Guid>> SeedWorkers(ServiceProvider provider, int count)
        {
            var guids = new List<Guid>();
            for (var i = 0; i < count; i++)
            {
                var command = new CreateWorkerCommand(Guid.NewGuid(), i.ToString());
                var result = await provider.GetRequiredService<ICreateWorkerUseCase>().HandleAsync(command);
                guids.Add(command.Id);
            }

            return guids;
        }
    }
}
