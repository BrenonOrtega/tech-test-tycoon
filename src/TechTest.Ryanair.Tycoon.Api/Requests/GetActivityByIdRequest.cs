using System.ComponentModel.DataAnnotations;

namespace TechTest.Ryanair.Tycoon.Api.Requests
{
    public class GetActivityByIdRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}