using Microsoft.AspNetCore.Mvc;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetBusiest;

namespace TechTest.Ryanair.Tycoon.Api.Requests.Workers;

public class GetBusiestWorkersRequest
{
    [FromQuery]
    public int Count { get; set; }

    [FromQuery]
    public DateTime StartDate { get; set; }

    [FromQuery]
    public DateTime EndDate { get; set; }

    internal GetBusiestWorkersCommand ToCommand() => new(Count, StartDate, EndDate);
}