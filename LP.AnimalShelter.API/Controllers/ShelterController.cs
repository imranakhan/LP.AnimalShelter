using LP.AnimalShelter.API.Enums;
using LP.AnimalShelter.API.Models;
using LP.AnimalShelter.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace LP.AnimalShelter.API.Controllers
{
    /// <summary>
    /// Shelter class to perform all the shelter GET, POST, PUT, DELETE operations
    /// </summary>
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

        /// <summary>
        /// Get All the Kennels that have Animals
        /// </summary>
        [HttpGet("kennels")]
        public async Task<IEnumerable<Kennel>> GetAllKennelsWithAnimals()
        {
            return _shelterService.GetAllKennelsWithAnimals();
        }

        /// <summary>
        /// Get all the Existing Animals in the shelter
        /// </summary>
        [HttpGet("animals")]
        public async Task<IEnumerable<Animal>> GetAllAnimals()
        {
            return _shelterService.GetAllAnimals();
        }

        /// <summary>
        /// Add an Animal to the Shelter based on best organization
        /// </summary>
        [HttpPost("animals")]
        public async Task<ActionResult<Animal>> AddAnimal(Animal model)
        {
            if (model == null) { return BadRequest(); }

            try
            {
                var animal = _shelterService.AddAnimal(model);
                return Ok(animal);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Delete an animal from the shelter as he/she has been adopted
        /// </summary>
        [HttpDelete("animals/{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            try
            {
                return Ok(_shelterService.RemoveAnimal(id));
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Reorganize all the animals in the kennels in the existing shelter. 
        /// This optimizes the space and frees up the bigger kennels where possible
        /// </summary>
        /// <returns></returns>
        [HttpPut("animals/reorganize")]
        public async Task<IActionResult> Reorganize()
        {
            _shelterService.ReorganizeAnimals();

            return Ok();
        }

        /// <summary>
        /// Add an animal to a specific size kennel
        /// </summary>
        [HttpPost("animals/customadd")]
        public async Task<IActionResult> AddAnimalToSpecificKennel(KennelType kennelType, [FromBody] Animal animal)
        {
            var result = _shelterService.AddAnimalToKennel(kennelType, animal);

            return Ok(result);
        }

        /// <summary>
        /// Generate sample data with Animals for API testing
        /// </summary>
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
