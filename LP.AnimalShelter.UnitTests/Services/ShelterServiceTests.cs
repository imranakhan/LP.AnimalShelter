using LP.AnimalShelter.API.Enums;
using LP.AnimalShelter.API.Models;
using LP.AnimalShelter.API.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace LP.AnimalShelter.Tests
{
    [TestClass]
    public class ShelterServiceTests
    {
        private Shelter _shelter;
        private Mock<ILogger<ShelterService>> _loggerMock;
        private ShelterService _service;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ShelterService>>();
            _shelter = new Shelter(2, 2, 2);

            _service = new ShelterService(_shelter, _loggerMock.Object);
        }

        [TestMethod]
        public void AddAnimal_AssignsToCorrectKennel()
        {
            // Arrange
            var animal = new Animal { Name = "Doggy", Type = "Dog", SizeInPounds = 15 };

            // Act
            var result = _service.AddAnimal(animal);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(animal.Name, result.Name);
            Assert.IsTrue(_shelter.Kennels.Where(x => x.Type == KennelType.Small).Count() == 2);
            Assert.IsTrue(_shelter.Kennels.Where(x => x.IsAvailable && x.Type == KennelType.Small).Count() == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "No more Available Kennels.")]
        public void AddAnimal_NoAvailableKennels_ThrowsException()
        {
            // Arrange
            _shelter = new Shelter(0, 0, 0);
            _service = new ShelterService(_shelter, _loggerMock.Object);

            var animal = new Animal { Name = "Doggy", Type = "Dog", SizeInPounds = 15 };

            // Act
            _service.AddAnimal(animal);
        }

        [TestMethod]
        public void GetAllAnimals_ReturnsAllOccupiedKennelAnimals()
        {
            // Arrange
            var animal = new Animal { Name = "Doggy", Type = "Dog", SizeInPounds = 35 };
            var animal2 = new Animal { Name = "Catty", Type = "Cat", SizeInPounds = 15 };
            _service.AddAnimal(animal);
            _service.AddAnimal(animal2);

            // Act
            var result = _service.GetAllAnimals();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetAllKennelsWithAnimals_ReturnsOccupiedKennels()
        {
            // Arrange
            var animal = new Animal { Name = "Doggy", Type = "Dog", SizeInPounds = 15 };
            var animal2 = new Animal { Name = "Catty", Type = "Cat", SizeInPounds = 35 };

            _service.AddAnimal(animal);
            _service.AddAnimal(animal2);


            // Act
            var result = _service.GetAllKennelsWithAnimals();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(animal.Name, result.FirstOrDefault(x => x.Type == KennelType.Small).Animal.Name);
            Assert.AreEqual(animal2.Name, result.FirstOrDefault(x => x.Type == KennelType.Medium).Animal.Name);
        }

        [TestMethod]
        public void RemoveAnimal_ValidId_RemovesAnimalFromKennel()
        {
            // Arrange
            var animal = new Animal { Name = "Doggy", Type = "Dog", SizeInPounds = 15 };
            var addedAnimal = _service.AddAnimal(animal);

            // Act
            var result = _service.RemoveAnimal(addedAnimal.Id);

            // Assert
            Assert.AreEqual(animal.Name, result.Name);
            Assert.IsTrue(_shelter.Kennels.Where(x => x.IsAvailable).Count() == 6);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Could not find Animal")]
        public void RemoveAnimal_InvalidId_ThrowsException()
        {
            // Act
            _service.RemoveAnimal(999);
        }

        [TestMethod]
        public void AddAnimalToKennel_AssignsAnimal()
        {
            // Arrange
            var animal = new Animal { Name = "Bulldog", Type = "Dog", SizeInPounds = 40 };

            // Act
            var result = _service.AddAnimalToKennel(KennelType.Medium, animal);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(animal.Name, result.Name);
            Assert.IsTrue(_shelter.Kennels.Where(k => k.Type == KennelType.Medium && !k.IsAvailable).Count() == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Size of Kennel is not available")]
        public void AddAnimalToKennel_NoAvailableKennel_ThrowsException()
        {
            // Arrange
            _shelter = new Shelter(2, 0, 2);
            _service = new ShelterService(_shelter, _loggerMock.Object);

            var animal = new Animal { Name = "Bulldog", Type = "Dog", SizeInPounds = 40 };

            // Act
            _service.AddAnimalToKennel(KennelType.Medium, animal);
        }

        [TestMethod]
        public void ReorganizeAnimals_ReorganizesAnimalsToEfficientKennels()
        {
            // Arrange
            _shelter = new Shelter(6, 6, 6);
            _service = new ShelterService(_shelter, _loggerMock.Object);

            var animal1 = new Animal { Name = "SmallDog", Type = "Dog", SizeInPounds = 15 };
            var animal2 = new Animal { Name = "MediumDog", Type = "Dog", SizeInPounds = 25 };
            var animal3 = new Animal { Name = "LargeDog", Type = "Dog", SizeInPounds = 55 };
            var animal4 = new Animal { Name = "SmallCat", Type = "Cat", SizeInPounds = 10 };
            var animal5 = new Animal { Name = "MediumCat", Type = "Cat", SizeInPounds = 35 };
            var animal6 = new Animal { Name = "LargeCat", Type = "Cat", SizeInPounds = 55 };

            _service.AddAnimalToKennel(KennelType.Medium, animal1);
            _service.AddAnimalToKennel(KennelType.Large, animal2);
            _service.AddAnimalToKennel(KennelType.Large, animal3);
            _service.AddAnimalToKennel(KennelType.Medium, animal4);
            _service.AddAnimalToKennel(KennelType.Large, animal5);
            _service.AddAnimalToKennel(KennelType.Large, animal6);

            // Act
            _service.ReorganizeAnimals();

            // Assert
            Assert.IsTrue(_shelter.Kennels.Count(x => !x.IsAvailable && x.Type == KennelType.Small) == 2);
            Assert.IsTrue(_shelter.Kennels.Count(x => !x.IsAvailable && x.Type == KennelType.Medium) == 2);
            Assert.IsTrue(_shelter.Kennels.Count(x => !x.IsAvailable && x.Type == KennelType.Large) == 2);
        }
    }
}
