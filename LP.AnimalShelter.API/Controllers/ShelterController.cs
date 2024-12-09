using LP.AnimalShelter.API.Enums;
using LP.AnimalShelter.API.Models;
using LP.AnimalShelter.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet("kennels")]
        public async Task<IEnumerable<Kennel>> GetAllKennelsWithAnimals()
        {
            return _shelterService.GetAllKennelsWithAnimals();
        }

        /// <summary>
        /// Retrieve all the Existing Animals in the shelter
        /// </summary>
        [HttpGet("animals")]
        public async Task<IEnumerable<Animal>> GetAllAnimals()
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
            _shelterService.ReorganizeAnimals();

            return Ok();
        }

        /// <summary>
        /// Add an animal to a specific size kennel
        /// </summary>
        /// <param name="kennelType"></param>
        /// <param name="animal"></param>
        /// <returns></returns>
        [HttpPost("animals/customadd")]
        public async Task<IActionResult> AddAnimalToSpecificKennel(KennelType kennelType, [FromBody] Animal animal)
        {
            var result = _shelterService.AddAnimalToKennel(kennelType, animal);

            return Ok(result);
        }

        [HttpPost("generatesampledata")]
        public async Task<IActionResult> SetupSampleData()
        {
            _shelterService.AddAnimal(new Animal
            {
                Type = "Dog",
                Name = "Doggy",
                SizeInPounds = 15
            });
            _shelterService.AddAnimal(new Animal
            {
                Type = "Cat",
                Name = "Catty",
                SizeInPounds = 35
            });
            _shelterService.AddAnimal(new Animal
            {
                Type = "Dog",
                Name = "Kitty",
                SizeInPounds = 25
            });

            _shelterService.AddAnimalToKennel(KennelType.Large, new Animal
            {
                Type = "Dog",
                Name = "Bulldog",
                SizeInPounds = 15
            });
            _shelterService.AddAnimalToKennel(KennelType.Large, new Animal
            {
                Type = "Dog",
                Name = "Husky",
                SizeInPounds = 25
            });
            _shelterService.AddAnimalToKennel(KennelType.Medium, new Animal
            {
                Type = "Goat",
                Name = "Goaty",
                SizeInPounds = 18
            });
            _shelterService.AddAnimalToKennel(KennelType.Medium, new Animal
            {
                Type = "Cat",
                Name = "Tom",
                SizeInPounds = 15
            });


            return Ok();
        }
    }
}
