using System.Collections.Immutable;
using TechTest.Ryanair.Tycoon.Domain.Entities;

namespace TechTest.Ryanair.Tycoon.Application.Dtos;

public record ActivityDto(Guid Id, DateTime Start, DateTime Finish, 
    TimeSpan Duration, TimeSpan Rest, string Type, ImmutableArray<Guid> Workers)
{
    public static readonly ActivityDto Null = new(Guid.Empty, DateTime.MinValue, DateTime.MaxValue, 
        TimeSpan.MaxValue, TimeSpan.MaxValue, "None", Array.Empty<Guid>().ToImmutableArray());

    internal static ActivityDto FromEntity(TimedActivity act) => new(act.Id, act.Start, act.Finish, 
        act.Duration, act.RestPeriod, act.Type, act.Workers.ToImmutableArray());
}
