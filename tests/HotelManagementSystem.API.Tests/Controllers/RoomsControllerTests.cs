using HotelManagementSystem.API.Controllers;
using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Interfaces.Services;
using HotelManagementSystem.Common.Models.Request;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HotelManagementSystem.API.Tests.Controllers
{
    public class RoomsControllerTests
    {
        private readonly Mock<IRoomsService> _mockRoomsService;
        private readonly RoomsController _controller;

        public RoomsControllerTests()
        {
            _mockRoomsService = new Mock<IRoomsService>();
            _controller = new RoomsController(_mockRoomsService.Object);
        }

        [Fact]
        public async Task Post_ValidRoomRequest_ReturnsOkResult()
        {
            // Arrange
            var roomRequest = new RoomRequest { };
            var roomResponse = new Room { };

            _mockRoomsService.Setup(service => service.CreateAsync(roomRequest))
                             .ReturnsAsync(roomResponse);

            // Act
            var result = await _controller.Post(roomRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRoom = Assert.IsType<Room>(okResult.Value);
            Assert.Equal(roomResponse, returnedRoom);
        }
    }
}
