using LP.AnimalShelter.API.Models;
using LP.AnimalShelter.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LP.AnimalShelter.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShelterController : ControllerBase
    {
        private readonly ILogger<ShelterController> _logger;
        private IShelterService _shelterService;

        public ShelterController(ILogger<ShelterController> logger, IShelterService shelterService)
        {
            _logger = logger;
            _shelterService = shelterService;
        }

        [HttpGet("animals")]
        public async Task<IEnumerable<Animal>> GetAll()
        {
            return _shelterService.GetAllAnimals();
        }

        [HttpPost("animals")]
        public async Task<ActionResult<Animal>> AddAnimal(Animal model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var animal = _shelterService.AddAnimal(model);

            return Ok(animal);
        }

        [HttpDelete("animals/{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            return Ok(_shelterService.RemoveAnimal(id));
        }

        [HttpPut("animals/reorganize")]
        public async Task<IActionResult> Reorganize()
        {
            return Ok();
        }
    }
}
