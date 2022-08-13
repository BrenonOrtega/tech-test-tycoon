using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TechTest.Ryanair.Tycoon.Application.WorkerUseCases.CreateWorker;

namespace TechTest.Ryanair.Tycoon.Api.Requests.Workers;

[BindProperties]
public class CreateWorkerRequest
{
    [FromHeader]
    [JsonPropertyName("worker-id")]
    public Guid? Id { get; set; }

    [Required]
    [FromBody]
    public string Name { get; set; }

    public CreateWorkerCommand ToCommand() => new CreateWorkerCommand(Id ?? Guid.NewGuid(), Name);
}
