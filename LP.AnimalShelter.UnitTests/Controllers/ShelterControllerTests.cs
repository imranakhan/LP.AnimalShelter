using LP.AnimalShelter.API.Controllers;
using LP.AnimalShelter.API.Enums;
using LP.AnimalShelter.API.Models;
using LP.AnimalShelter.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LP.AnimalShelter.Tests
{
    [TestClass]
    public class ShelterControllerTests
    {
        private Mock<ILogger<ShelterController>> _loggerMock;
        private Mock<IShelterService> _shelterServiceMock;
        private ShelterController _controller;

        [TestInitialize]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<ShelterController>>();
            _shelterServiceMock = new Mock<IShelterService>();
            _controller = new ShelterController(_loggerMock.Object, _shelterServiceMock.Object);
        }

        [TestMethod]
        public async Task GetAllKennelsWithAnimals_ReturnsAllOccupiedKennels()
        {
            // Arrange
            var kennels = new List<Kennel>
            {
                new Kennel { Animal = new Animal(), Type = KennelType.Large },
                new Kennel { Animal = new Animal(), Type = KennelType.Medium }
            };
            _shelterServiceMock.Setup(s => s.GetAllKennelsWithAnimals()).Returns(kennels);

            // Act
            var result = await _controller.GetAllKennelsWithAnimals();

            // Assert
            Assert.AreEqual(kennels.Count(), result.Count());
            _shelterServiceMock.Verify(s => s.GetAllKennelsWithAnimals(), Times.Once);
        }

        [TestMethod]
        public async Task GetAllAnimals_ReturnsAllAnimals()
        {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal { Id = 1, Name = "Doggy", Type = "Dog", SizeInPounds = 15 },
                new Animal { Id = 2, Name = "Catty", Type = "Cat", SizeInPounds = 10 }
            };
            _shelterServiceMock.Setup(s => s.GetAllAnimals()).Returns(animals);

            // Act
            var result = await _controller.GetAllAnimals();

            // Assert
            Assert.AreEqual(animals, result);
            _shelterServiceMock.Verify(s => s.GetAllAnimals(), Times.Once);
        }

        [TestMethod]
        public async Task AddAnimal_ReturnsOkResult()
        {
            // Arrange
            var animal = new Animal { Id = 1, Name = "Doggy", Type = "Dog", SizeInPounds = 15 };
            _shelterServiceMock.Setup(s => s.AddAnimal(animal)).Returns(animal);

            // Act
            var result = await _controller.AddAnimal(animal);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(animal, okResult.Value);
            _shelterServiceMock.Verify(s => s.AddAnimal(animal), Times.Once);
        }

        [TestMethod]
        public async Task AddAnimal_NullModel_ReturnsBadRequest()
        {
            // Act
            var result = await _controller.AddAnimal(null);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task DeleteAnimal_ReturnsOkResult()
        {
            // Arrange
            var animal = new Animal { Id = 1, Name = "Doggy", Type = "Dog", SizeInPounds = 15 };
            _shelterServiceMock.Setup(s => s.RemoveAnimal(animal.Id)).Returns(animal);

            // Act
            var result = await _controller.DeleteAnimal(animal.Id);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(animal, okResult.Value);
            _shelterServiceMock.Verify(s => s.RemoveAnimal(animal.Id), Times.Once);
        }

        [TestMethod]
        public async Task Reorganize_ReturnsOkResult()
        {
            // Act
            var result = await _controller.Reorganize();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _shelterServiceMock.Verify(s => s.ReorganizeAnimals(), Times.Once);
        }

        [TestMethod]
        public async Task AddAnimalToSpecificKennel_ReturnsOkResult()
        {
            // Arrange
            var kennelType = KennelType.Large;
            var animal = new Animal { Id = 1, Name = "Doggy", Type = "Dog", SizeInPounds = 15 };
            _shelterServiceMock.Setup(s => s.AddAnimalToKennel(kennelType, animal)).Returns(animal);

            // Act
            var result = await _controller.AddAnimalToSpecificKennel(kennelType, animal);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(animal, okResult.Value);
            _shelterServiceMock.Verify(s => s.AddAnimalToKennel(kennelType, animal), Times.Once);
        }

        [TestMethod]
        public async Task SetupSampleData_ReturnsOkResult()
        {
            // Act
            var result = await _controller.SetupSampleData();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _shelterServiceMock.Verify(s => s.AddAnimal(It.IsAny<Animal>()), Times.Exactly(3));
            _shelterServiceMock.Verify(s => s.AddAnimalToKennel(It.IsAny<KennelType>(), It.IsAny<Animal>()), Times.Exactly(4));
        }
    }
}