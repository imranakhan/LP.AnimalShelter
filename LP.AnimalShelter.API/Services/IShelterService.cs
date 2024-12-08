using LP.AnimalShelter.API.Models;

namespace LP.AnimalShelter.API.Services
{
    public interface IShelterService
    {
        public Animal AddAnimal(Animal animal);
        public Animal RemoveAnimal(int id);
        public Animal GetAnimal(int id);
        public List<Animal> GetAllAnimals();
        public void ReorganizeAnimals();
    }
}
