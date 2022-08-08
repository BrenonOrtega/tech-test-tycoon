using Microsoft.Extensions.DependencyInjection;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;
using TechTest.Ryanair.Tycoon.Infra.Repositories;

namespace TechTest.Ryanair.Tycoon.Infra.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddActivityRepository();
        services.AddWorkerRepository();

        return services;
    }

    public static IServiceCollection AddActivityRepository(this IServiceCollection services)
    {
        services.AddSingleton<Dictionary<Guid, TimedActivity>>();
        return services.AddScoped<ITimedActivityRepository, TimedActivityInMemoryRepository>();
    }

    public static IServiceCollection AddWorkerRepository(this IServiceCollection services)
    {
        services.AddSingleton<Dictionary<Guid, Worker>>();
        return services.AddScoped<IWorkerRepository, WorkerInMemoryRepository>();
    }
}
