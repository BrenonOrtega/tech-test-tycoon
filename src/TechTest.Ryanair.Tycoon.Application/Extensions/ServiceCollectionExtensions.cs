using Microsoft.Extensions.DependencyInjection;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;
using TechTest.Ryanair.Tycoon.Domain.Factories;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;

namespace TechTest.Ryanair.Tycoon.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScheduleActivityUseCase();
        services.AddCreateActivityUseCase();
        
        services.AddCreateWorkerUseCase();
        services.AddGetWorkerByIdUseCase();

        services. AddFactories();

        return services;
    }

    public static IServiceCollection AddScheduleActivityUseCase(this IServiceCollection services)
    {
        return services.AddScoped<IScheduleActivityUseCase, ScheduleActivityUseCase>();
    }

    public static IServiceCollection AddCreateActivityUseCase(this IServiceCollection services)
    {
        return services.AddScoped<ICreateActivityUseCase, CreateActivityUseCase>();
    }

    public static IServiceCollection AddCreateWorkerUseCase(this IServiceCollection services)
    {
        return services.AddScoped<ICreateWorkerUseCase, CreateWorkerUseCase>();
    }

    public static IServiceCollection AddGetWorkerByIdUseCase(this IServiceCollection services)
    {
        return services.AddScoped<IGetWorkerByIdUseCase, GetWorkerByIdUseCase>();
    }

    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        return services.AddScoped<IActivityFactory, ActivityFactory>();
    }
}
