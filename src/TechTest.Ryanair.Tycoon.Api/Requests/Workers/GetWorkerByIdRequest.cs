using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.GetWorkerById;

namespace TechTest.Ryanair.Tycoon.Api.Requests.Workers;

public class GetWorkerByIdRequest
{
    [Required]
    public Guid Id { get; set; }

    internal GetWorkerByIdCommand ToCommand() => new(Id);
}