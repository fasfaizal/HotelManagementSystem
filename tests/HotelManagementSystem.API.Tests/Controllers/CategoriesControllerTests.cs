using HotelManagementSystem.API.Controllers;
using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Enums;
using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelManagementSystem.API.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoriesService> _mockCategoriesService;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _mockCategoriesService = new Mock<ICategoriesService>();
            _controller = new CategoriesController(_mockCategoriesService.Object);
        }

        [Fact]
        public async Task Post_ValidCategoryRequest_ReturnsCreatedResponse()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { };
            var createdCategory = new Category { };

            _mockCategoriesService
                .Setup(service => service.CreateAsync(categoryRequest))
                .ReturnsAsync(createdCategory);

            // Act
            var result = await _controller.Post(categoryRequest);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.IsType<Category>(createdResult.Value);
        }

        [Fact]
        public async Task Post_ValidCategoryRequest_CallsCreateAsyncOnce()
        {
            // Arrange
            var categoryRequest = new CategoryRequest { /* initialize properties */ };

            _mockCategoriesService
                .Setup(service => service.CreateAsync(categoryRequest))
                .ReturnsAsync(new Category { /* initialize properties */ });

            // Act
            await _controller.Post(categoryRequest);

            // Assert
            _mockCategoriesService.Verify(service => service.CreateAsync(categoryRequest), Times.Once);
        }

        [Fact]
        public async Task Get_ValidId_ReturnsOkResponse()
        {
            // Arrange
            var categoryId = "123";
            var category = new Category
            {
                CategoryName = "Luxury",
                Capacity = 10,
                PricePerNight = 150.00m,
                BedType = BedType.Queen
            };

            _mockCategoriesService
                .Setup(service => service.GetByIdAsync(categoryId))
                .ReturnsAsync(category);

            // Act
            var result = await _controller.Get(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(category, okResult.Value);
        }

        [Fact]
        public async Task Get_InvalidId_ReturnsNotFoundResponse()
        {
            // Arrange
            var categoryId = "123";

            _mockCategoriesService
                .Setup(service => service.GetByIdAsync(categoryId))
                .ReturnsAsync((Category)null);

            // Act
            var result = await _controller.Get(categoryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
    }
}
