using HotelManagementSystem.API.Controllers;
using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;
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
    }
}
