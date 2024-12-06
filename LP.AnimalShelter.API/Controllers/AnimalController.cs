using LP.AnimalShelter.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LP.AnimalShelter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelterController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<ShelterController> _logger;

        public ShelterController(ILogger<ShelterController> logger)
        {
            _logger = logger;
        }

        [HttpGet("animals")]
        public IEnumerable<WeatherForecast> GetAll()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("animals")]
        public async Task<ActionResult<Animal>> AddAnimal(Animal model)
        {
            return Ok(model);
        }

        [HttpDelete("animals/{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            return Ok();
        }

        [HttpPut("animals/reorganize")]
        public async Task<IActionResult> ReOrganize()
        {
            return Ok();
        }
    }
}
