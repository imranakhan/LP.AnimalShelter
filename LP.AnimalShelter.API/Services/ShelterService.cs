using LP.AnimalShelter.API.Models;

namespace LP.AnimalShelter.API.Services
{
    public class ShelterService : IShelterService
    {
        // Using a singleton instance of a shelter object to persist data temporarily
        private Shelter _shelter;

        public ShelterService(Shelter shelter)
        { 
            _shelter = shelter;
        }

        public Animal AddAnimal(Animal animal)
        {
            if (_shelter.ExistingAnimalIds.Contains(animal.Id))
            {
                Console.WriteLine("Animal Already exists in shelter. Skipping");
            }
            else
            {
                Random rnd = new Random();
                animal.Id = rnd.Next(1, 9999999);
                if (!_shelter.HasAvailableKennels)
                {
                    return null;
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

        public void ReorganizeAnimals()
        {
            throw new NotImplementedException();
        }
    }
}
