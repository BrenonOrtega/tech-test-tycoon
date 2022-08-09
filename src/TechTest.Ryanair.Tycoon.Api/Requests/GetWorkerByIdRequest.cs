using Microsoft.AspNetCore.Mvc;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;

namespace TechTest.Ryanair.Tycoon.Api.Requests;

public class GetWorkerByIdRequest
{
    [FromRoute]
    public Guid Id { get; set; }

    internal GetWorkerByIdCommand ToCommand() => new(Id);
}