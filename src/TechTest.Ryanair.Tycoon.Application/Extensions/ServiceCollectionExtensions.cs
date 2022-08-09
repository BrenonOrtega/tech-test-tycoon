using Microsoft.Extensions.DependencyInjection;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.CreateActivity;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.ScheduleActivity;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;
using TechTest.Ryanair.Tycoon.Domain.Factories;

namespace TechTest.Ryanair.Tycoon.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScheduleActivity();
        services.AddCreateActivity();
        services.AddGetActivityById();

        services.AddCreateWorker();
        services.AddGetWorkerById();

        services.AddFactories();

        return services;
    }

    public static IServiceCollection AddScheduleActivity(this IServiceCollection services)
    {
        return services.AddScoped<IScheduleActivityUseCase, ScheduleActivityUseCase>();
    }

    public static IServiceCollection AddCreateActivity(this IServiceCollection services)
    {
        return services.AddScoped<ICreateActivityUseCase, CreateActivityUseCase>();
    }

    public static IServiceCollection AddGetActivityById(this IServiceCollection services)
    {
        return services.AddScoped<IGetActivityByIdUseCase, GetActivityByIdUseCase>();
    }

    public static IServiceCollection AddCreateWorker(this IServiceCollection services)
    {
        return services.AddScoped<ICreateWorkerUseCase, CreateWorkerUseCase>();
    }

    public static IServiceCollection AddGetWorkerById(this IServiceCollection services)
    {
        return services.AddScoped<IGetWorkerByIdUseCase, GetWorkerByIdUseCase>();
    }

    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        return services.AddScoped<IActivityFactory, ActivityFactory>();
    }
}
