using System.ComponentModel.DataAnnotations;
using TechTest.Ryanair.Tycoon.Application.ActivitiesUseCases.GetActivityById;

namespace TechTest.Ryanair.Tycoon.Api.Requests
{
    public class GetActivityByIdRequest
    {
        [Required]
        public Guid Id { get; set; }

        internal GetActivityByIdCommand ToCommand() => new(Id);
    }
}