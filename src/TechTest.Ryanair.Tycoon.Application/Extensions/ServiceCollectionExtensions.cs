using Microsoft.Extensions.DependencyInjection;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;

namespace TechTest.Ryanair.Tycoon.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScheduleActivityUseCase();
        
        services.AddCreateWorkerUseCase();
        services.AddGetWorkerByIdUseCase();

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

    public static IServiceCollection AddGetWorkerByIdUseCase(this IServiceCollection services)
    {
        return services.AddScoped<IGetWorkerByIdUseCase, GetWorkerByIdUseCase>();
    }
}
