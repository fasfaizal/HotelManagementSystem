using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Enums;
using HotelManagementSystem.Common.Exceptions;
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

        [Fact]
        public async Task GetCategoriesAsync_ValidParameters_ReturnsCategories()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var expectedCategories = new List<Category>
            {
                new Category { CategoryName = "Category1" },
                new Category { CategoryName = "Category2" }
            };

            _mockRepo.Setup(repo => repo.GetCategoriesAsync(pageNumber, pageSize))
                     .ReturnsAsync(expectedCategories);

            // Act
            var result = await _service.GetCategoriesAsync(pageNumber, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCategories, result);
            _mockRepo.Verify(repo => repo.GetCategoriesAsync(pageNumber, pageSize), Times.Once);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 0)]
        [InlineData(-1, 10)]
        [InlineData(1, -10)]
        public async Task GetCategoriesAsync_InvalidParameters_ThrowsApiException(int pageNumber, int pageSize)
        {
            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _service.GetCategoriesAsync(pageNumber, pageSize));
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal("Parameters should be positive", exception.Message);
        }
    }
}
