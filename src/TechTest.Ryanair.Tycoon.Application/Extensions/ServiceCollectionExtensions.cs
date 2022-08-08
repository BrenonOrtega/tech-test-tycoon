using Microsoft.Extensions.DependencyInjection;
using TechTest.Ryanair.Tycoon.Application.CreateWorker;
using TechTest.Ryanair.Tycoon.Application.ScheduleActivity;

namespace TechTest.Ryanair.Tycoon.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScheduleActivityUseCase();
        services.AddCreateWorkerUseCase();

        return services;
    }

    public static IServiceCollection AddScheduleActivityUseCase(this IServiceCollection services)
    {
        return services.AddScoped<IScheduleActivityUseCase, ScheduleActivityUseCase>();
    }

    public static IServiceCollection AddCreateWorkerUseCase(this IServiceCollection services)
    {
        return services.AddScoped<ICreateWorkerUseCase, CreateWorkerUseCase>();
    }
}
