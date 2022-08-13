using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;

namespace TechTest.Ryanair.Tycoon.Api.Requests.Activities
{
    public class GetActivityByIdRequest
    {
        [Required]
        [FromRoute]
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        internal GetActivityByIdCommand ToCommand() => new(Id);
    }
}