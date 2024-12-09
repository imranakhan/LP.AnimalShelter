using LP.AnimalShelter.API.Controllers;
using LP.AnimalShelter.API.Enums;
using LP.AnimalShelter.API.Models;

namespace LP.AnimalShelter.API.Services
{
    public class ShelterService : IShelterService
    {
        // Using a singleton instance of a shelter object to persist data temporarily
        private Shelter _shelter;
        private readonly ILogger<ShelterService> _logger;

        public ShelterService(Shelter shelter, ILogger<ShelterService> logger)
        { 
            _shelter = shelter;
            _logger = logger;
        }

        public Animal AddAnimal(Animal animal)
        {
            if (_shelter.ExistingAnimalIds.Contains(animal.Id))
            {
                _logger.Log(LogLevel.Information, "Animal Already exists in shelter. Skipping");
                return _shelter.Kennels.FirstOrDefault(x => x.Animal != null && x.Animal.Id == animal.Id)?.Animal;
            }
            else
            {
                animal.Id = GenerateRandomId();
                if (!_shelter.HasAvailableKennels)
                {
                    throw new Exception("No more Available Kennels.");
                }

                Kennel availableKennel = null;
                if (animal.SizeInPounds <= 20)
                {
                    if (_shelter.HasAvailableSmallKennels)
                    {
                        availableKennel = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Small);
                    } 
                    else if(_shelter.HasAvailableMediumKennels)
                    {
                        availableKennel = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Medium);
                    }
                    else if(_shelter.HasAvailableLargeKennels)
                    {
                        availableKennel = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Large);
                    }
                }
                else if (animal.SizeInPounds <= 50)
                {
                    if (_shelter.HasAvailableMediumKennels)
                    {
                        availableKennel = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Medium);
                    }
                    else if (_shelter.HasAvailableLargeKennels)
                    {
                        availableKennel = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Large);
                    }
                }
                else
                {
                    if (_shelter.HasAvailableLargeKennels)
                    {
                        availableKennel = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Large);
                    }
                }

                if (availableKennel != null)
                {
                    availableKennel.Animal = animal;
                }
                else
                {
                    throw new Exception("No appropriate kennel found for the animal");
                }
            }

            return animal;
        }

        public List<Animal> GetAllAnimals()
        {
            return _shelter.Kennels.Where(x => !x.IsAvailable).Select(x => x.Animal).ToList();
        }

        public List<Kennel> GetAllKennelsWithAnimals()
        {
            return _shelter.Kennels.Where(x => !x.IsAvailable).ToList();
        }

        public Animal GetAnimal(int id)
        {
            var existingAnimalsKennel = _shelter.Kennels.FirstOrDefault(x => x.Animal != null && x.Animal.Id == id);

            if (existingAnimalsKennel != null)
            {
                return existingAnimalsKennel.Animal;
            }
            else
            {
                return null;
            }
        }

        public Animal RemoveAnimal(int id)
        {
            var existingAnimalsKennel = _shelter.Kennels.FirstOrDefault(x => x.Animal != null && x.Animal.Id == id);
            if (existingAnimalsKennel != null)
            {
                var existingAnimal  = existingAnimalsKennel.Animal;
                existingAnimalsKennel.Animal = null;
                return existingAnimal;
            }
            else
            {
                throw new Exception("Could not find Animal");
            }           
        }

        public Animal AddAnimalToKennel(KennelType type, Animal animal)
        {
            var findAvailableKennel = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == type);

            if (findAvailableKennel != null)
            {
                animal.Id = GenerateRandomId();
                findAvailableKennel.Animal = animal;
            }
            else
            {
                throw new Exception("Size of Kennel is not available");
            }

            return animal;
        }

        private int GenerateRandomId()
        {
            Random rnd = new Random();
            return rnd.Next(1, 9999999);
        }

        public void ReorganizeAnimals()
        {
            // Skip the animals in the small Kennels as they are already in the most efficient spot
            // For animals in medium kennels, see if there are animals that can fit in small kennels that are available.
            var mediumKennelsWithAnimals = _shelter.Kennels.Where(x => !x.IsAvailable && x.Type == Enums.KennelType.Medium);

            if (mediumKennelsWithAnimals.Any() && _shelter.HasAvailableSmallKennels)
            {
                foreach(var kennel in mediumKennelsWithAnimals)
                {
                    var animal = kennel.Animal;

                    if (animal.SizeInPounds <= 20 && _shelter.HasAvailableSmallKennels)
                    {
                        var newKennelforAnimal = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Small);
                        if (newKennelforAnimal != null)
                        {
                            // Move to new kennel
                            newKennelforAnimal.Animal = animal;
                            kennel.Animal = null;
                        }
                    }
                }
            }

            // For animals in large kennels, see if they can fit in small kennels, if not then medium kennels.
            var largeKennelsWithAnimals = _shelter.Kennels.Where(x => !x.IsAvailable && x.Type == Enums.KennelType.Large);

            if (largeKennelsWithAnimals.Any())
            {
                foreach (var kennel in largeKennelsWithAnimals)
                {
                    var animal = kennel.Animal;

                    Kennel newKennelForAnimal = null;

                    if (animal.SizeInPounds <= 20)
                    {
                        if (_shelter.HasAvailableSmallKennels)
                        {
                            newKennelForAnimal = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Small);
                        }
                        else if (_shelter.HasAvailableMediumKennels)
                        {
                            newKennelForAnimal = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Medium);
                        }
                    }
                    else if(animal.SizeInPounds > 20
                        && animal.SizeInPounds <= 50 
                        && _shelter.HasAvailableMediumKennels)
                    {
                        newKennelForAnimal = _shelter.Kennels.FirstOrDefault(x => x.IsAvailable && x.Type == Enums.KennelType.Medium);
                    }

                    if (newKennelForAnimal != null)
                    {
                        // Move to new kennel
                        newKennelForAnimal.Animal = animal;
                        kennel.Animal = null;
                    }
                }
            }
        }
    }
}
