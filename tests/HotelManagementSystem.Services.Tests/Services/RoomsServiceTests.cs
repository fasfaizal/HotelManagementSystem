using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Exceptions;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.Common.Models.Request;
using HotelManagementSystem.Services.Services;
using Moq;
using System.Net;

namespace HotelManagementSystem.Services.Tests.Services
{
    public class RoomsServiceTests
    {
        private readonly Mock<IRoomsRepo> _mockRoomsRepo;
        private readonly Mock<ICategoriesRepo> _mockCategoriesRepo;
        private readonly RoomsService _roomsService;

        public RoomsServiceTests()
        {
            _mockRoomsRepo = new Mock<IRoomsRepo>();
            _mockCategoriesRepo = new Mock<ICategoriesRepo>();
            _roomsService = new RoomsService(_mockRoomsRepo.Object, _mockCategoriesRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_RoomRequestIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            RoomRequest roomRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _roomsService.CreateAsync(roomRequest));
        }

        [Fact]
        public async Task CreateAsync_InvalidCategory_ThrowsApiException()
        {
            // Arrange
            var roomRequest = new RoomRequest
            {
                CategoryId = "CategoryId",
                RoomName = "Room 1",
                SizeInSquareFeet = 500,
                FloorNumber = 2
            };
            _mockCategoriesRepo.Setup(repo => repo.GetByIdAsync(roomRequest.CategoryId)).ReturnsAsync((Category)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _roomsService.CreateAsync(roomRequest));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal("Invalid category", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ValidRoomRequest_CreatesRoom()
        {
            // Arrange
            var roomRequest = new RoomRequest
            {
                CategoryId = "CategoryId",
                RoomName = "Room 1",
                SizeInSquareFeet = 500,
                FloorNumber = 2
            };
            var category = new Category { };
            _mockCategoriesRepo.Setup(repo => repo.GetByIdAsync(roomRequest.CategoryId)).ReturnsAsync(category);
            _mockRoomsRepo.Setup(repo => repo.CreateAsync(It.IsAny<Room>())).Returns(Task.CompletedTask);

            // Act
            var result = await _roomsService.CreateAsync(roomRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(roomRequest.CategoryId, result.CategoryId);
            Assert.Equal(roomRequest.RoomName, result.RoomName);
            Assert.Equal(roomRequest.SizeInSquareFeet, result.SizeInSquareFeet);
            Assert.Equal(roomRequest.FloorNumber, result.FloorNumber);
            _mockRoomsRepo.Verify(repo => repo.CreateAsync(It.IsAny<Room>()), Times.Once);
        }
    }
}
