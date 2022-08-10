using Microsoft.AspNetCore.Mvc;
using TechTest.Ryanair.Tycoon.Domain.Entities;
using TechTest.Ryanair.Tycoon.Domain.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechTest.Ryanair.Tycoon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemonstrativePurposeController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public DemonstrativePurposeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet("activities")]
        public async Task<ActionResult<IEnumerable<TimedActivity>>> GetActivities()
        {
            return Ok(await unitOfWork.ActivityRepository.GetAllAsync());
        }

        // GET api/<DemonstrativePurposeController>/5
        [HttpGet("workers")]
        public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers()
        {
            return Ok(await unitOfWork.WorkerRepository.GetAllAsync());
        }  
    }
}
