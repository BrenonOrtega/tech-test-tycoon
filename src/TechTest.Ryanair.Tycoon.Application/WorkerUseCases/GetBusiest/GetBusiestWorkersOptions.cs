namespace TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;

public class GetBusiestWorkersOptions
{
    public int? GetQuantity { get; init; } 
    public DateTime? StartDate { get; init; }
    public DateTime? FinishDate { get; init; }

    public GetBusiestWorkersOptions()
    {
        GetQuantity = 20;
        FinishDate = DateTime.Now;
        StartDate = FinishDate - TimeSpan.FromDays(7);
    }
}
