using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Enums;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.Common.Models.Request;
using HotelManagementSystem.Services.Services;
using Moq;

namespace HotelManagementSystem.Services.Tests.Services
{
    public class CategoriesServiceTests
    {
        private readonly Mock<ICategoriesRepo> _mockRepo;
        private readonly CategoriesService _service;

        public CategoriesServiceTests()
        {
            _mockRepo = new Mock<ICategoriesRepo>();
            _service = new CategoriesService(_mockRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_ValidCategory_ReturnsCreatedCategory()
        {
            // Arrange
            var categoryRequest = new CategoryRequest
            {
                BedType = BedType.Twin,
                Capacity = 2,
                CategoryName = "Luxury Suite",
                Description = "Spacious suite with luxury amenities",
                PricePerNight = 300
            };

            // Act
            var result = await _service.CreateAsync(categoryRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(categoryRequest.BedType, result.BedType);
            Assert.Equal(categoryRequest.Capacity, result.Capacity);
            Assert.Equal(categoryRequest.CategoryName, result.CategoryName);
            Assert.Equal(categoryRequest.Description, result.Description);
            Assert.Equal(categoryRequest.PricePerNight, result.PricePerNight);

            _mockRepo.Verify(repo => repo.CreateAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_NullCategoryRequest_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateAsync(null));
        }
    }
}
