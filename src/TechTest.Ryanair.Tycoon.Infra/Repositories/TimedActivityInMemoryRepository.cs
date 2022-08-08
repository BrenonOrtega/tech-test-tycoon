using Microsoft.Extensions.Logging;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

namespace TechTest.Ryanair.Tycoon.Infra.Repositories;

internal class TimedActivityInMemoryRepository : BaseInMemoryRepository<TimedActivity, TimedActivityInMemoryRepository>, ITimedActivityRepository
{
    public TimedActivityInMemoryRepository(Dictionary<Guid, TimedActivity> activities, ILogger<TimedActivityInMemoryRepository> logger) : base(logger)
    {
        _activities = activities ?? throw new ArgumentNullException(nameof(activities));
    }

    private readonly Dictionary<Guid, TimedActivity> _activities = new();
    protected override Dictionary<Guid, TimedActivity> Data => _activities;
}
