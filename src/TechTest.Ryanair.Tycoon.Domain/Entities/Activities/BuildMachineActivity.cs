
namespace TechTest.Ryanair.Tycoon.Domain.Entities;
public class BuildMachineActivity : TimedActivity
{
    public BuildMachineActivity(Guid id, DateTime start, DateTime finish) : base(id, start, finish)
    {
    }

    public override TimeSpan RestTime => TimeSpan.FromHours(4);

    public override string Type => "Build Machine Activity";
}