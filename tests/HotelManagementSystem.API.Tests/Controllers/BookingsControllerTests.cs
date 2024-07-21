using HotelManagementSystem.API.Controllers;
using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelManagementSystem.API.Tests.Controllers
{
    public class BookingsControllerTests
    {
        private readonly Mock<IBookingsService> _mockBookingsService;
        private readonly BookingsController _controller;

        public BookingsControllerTests()
        {
            _mockBookingsService = new Mock<IBookingsService>();
            _controller = new BookingsController(_mockBookingsService.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOkResult_WhenBookingIsCreated()
        {
            // Arrange
            var bookingRequest = new BookingRequest();

            _mockBookingsService
                .Setup(service => service.CreateAsync(bookingRequest))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Post(bookingRequest);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            _mockBookingsService.Verify(service => service.CreateAsync(bookingRequest), Times.Once);
        }

        [Fact]
        public async Task GetAvailability_ShouldReturnOkResult_WithAvailabilityStatus()
        {
            // Arrange
            string categoryId = "123";
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            bool expectedAvailability = true;

            _mockBookingsService
                .Setup(service => service.IsAvailable(categoryId, startDate, endDate))
                .ReturnsAsync(expectedAvailability);

            // Act
            var result = await _controller.GetAvailability(categoryId, startDate, endDate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseValue = Assert.IsType<bool>(okResult.Value);
            Assert.Equal(expectedAvailability, responseValue);
            _mockBookingsService.Verify(service => service.IsAvailable(categoryId, startDate, endDate), Times.Once);
        }

        [Fact]
        public async Task GetAvailability_ShouldUseDefaultDates_WhenNotProvided()
        {
            // Arrange
            string categoryId = "123";
            DateTime today = DateTime.Today;
            bool expectedAvailability = true;

            _mockBookingsService
                .Setup(service => service.IsAvailable(categoryId, today, today))
                .ReturnsAsync(expectedAvailability);

            // Act
            var result = await _controller.GetAvailability(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var responseValue = Assert.IsType<bool>(okResult.Value);
            Assert.Equal(expectedAvailability, responseValue);
            _mockBookingsService.Verify(service => service.IsAvailable(categoryId, today, today), Times.Once);
        }
    }
}
