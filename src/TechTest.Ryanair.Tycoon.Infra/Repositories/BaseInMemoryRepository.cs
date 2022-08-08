using System.Text.Json;
using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Infra.Repositories;

public abstract class BaseInMemoryRepository<TEntity, TLogger> : IBaseRepository<TEntity>
{
    private readonly string entityType = typeof(TEntity).Name;
    protected readonly ILogger<TLogger> _logger;
    protected abstract Dictionary<Guid, TEntity> Data { get; }

    public BaseInMemoryRepository(ILogger<TLogger> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TEntity> GetAsync(Guid id)
    {
        if (Data.TryGetValue(id, out var entity))
        {
            _logger.LogInformation("Not found any record of type {type} with id {id}", entityType, id);
            return entity;
        }

        return default;
    }

    public async Task<Result> CreateAsync(TEntity entity)
    {
        if (entity is null)
        {
            _logger.LogInformation("Unable to save entity {entityType} value for entity was null.", entityType);
            return Result.Fail(InfrastructureErrors.NullEntity);
        }

        var id = typeof(TEntity).GetProperty("Id").GetValue(entity) as Guid?;
        if (id is null)
        {
            _logger.LogInformation("Entity {entityType} does not contain any Property named 'Id' to be used for query", entityType, id);
            return Result.Fail(InfrastructureErrors.InvalidEntityType);
        }

        _logger.LogInformation("Saving new {type} record for id {id} with data: {data}", entityType, id, JsonSerializer.Serialize(entity));
        Data.Add((Guid)id, entity);
        
        return Result.Success();
    }

    public async Task<Result> UpdateAsync(Guid id, TEntity updated)
    {
        if (updated is null)
            return Result.Fail(InfrastructureErrors.InvalidUpdateCommand);

        var exists = Data.ContainsKey(id);
        if (exists is false)
            return Result.Fail(InfrastructureErrors.EntityNotFound);

        Data[id] = updated;
        return Result.Success();
    }
}