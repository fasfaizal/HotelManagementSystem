using HotelManagementSystem.Common.Entities;
using HotelManagementSystem.Common.Exceptions;
using HotelManagementSystem.Common.Interfaces.DataAccess;
using HotelManagementSystem.Common.Models.Request;
using HotelManagementSystem.Services.Services;
using Moq;
using System.Net;

namespace HotelManagementSystem.Services.Tests.Services
{
    public class BookingsServiceTests
    {
        private readonly Mock<IBookingsRepo> _mockBookingsRepo;
        private readonly Mock<ICategoriesRepo> _mockCategoriesRepo;
        private readonly Mock<IRoomsRepo> _mockRoomsRepo;
        private readonly BookingsService _bookingsService;

        public BookingsServiceTests()
        {
            _mockBookingsRepo = new Mock<IBookingsRepo>();
            _mockCategoriesRepo = new Mock<ICategoriesRepo>();
            _mockRoomsRepo = new Mock<IRoomsRepo>();
            _bookingsService = new BookingsService(_mockBookingsRepo.Object, _mockCategoriesRepo.Object, _mockRoomsRepo.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowArgumentNullException_WhenBookingRequestIsNull()
        {
            // Arrange
            BookingRequest bookingRequest = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _bookingsService.CreateAsync(bookingRequest));
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowApiException_WhenCategoryIsInvalid()
        {
            // Arrange
            var bookingRequest = new BookingRequest { CategoryId = "invalid-category-id", NoOfGuests = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            _mockCategoriesRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync((Category)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _bookingsService.CreateAsync(bookingRequest));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal("Invalid category", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowApiException_WhenGuestsExceedCategoryCapacity()
        {
            // Arrange
            var bookingRequest = new BookingRequest { CategoryId = "valid-category-id", NoOfGuests = 5, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            var category = new Category { Id = "valid-category-id", Capacity = 4 };
            _mockCategoriesRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(category);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _bookingsService.CreateAsync(bookingRequest));
            Assert.Equal(HttpStatusCode.BadRequest, exception.StatusCode);
            Assert.Equal("Allowed guests :4", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowApiException_WhenNoRoomsAreAvailable()
        {
            // Arrange
            var bookingRequest = new BookingRequest { CategoryId = "valid-category-id", NoOfGuests = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            var category = new Category { Id = "valid-category-id", Capacity = 4 };
            _mockCategoriesRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(category);
            _mockRoomsRepo.Setup(repo => repo.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync(new List<Room> { new Room { CategoryId = "valid-category-id" } });
            _mockBookingsRepo.Setup(repo => repo.GetBookingsForRange(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Booking> { new Booking { CategoryId = "valid-category-id" } });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _bookingsService.CreateAsync(bookingRequest));
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
            Assert.Equal("No rooms available at this time", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateBooking_WhenValidRequest()
        {
            // Arrange
            var bookingRequest = new BookingRequest { CategoryId = "valid-category-id", NoOfGuests = 1, StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
            var category = new Category { Id = "valid-category-id", Capacity = 4 };
            var rooms = new List<Room> { new Room { Id = "room1" } };
            _mockCategoriesRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(category);
            _mockRoomsRepo.Setup(repo => repo.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync(rooms);
            _mockBookingsRepo.Setup(repo => repo.GetBookingsForRange(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Booking>());

            // Act
            await _bookingsService.CreateAsync(bookingRequest);

            // Assert
            _mockBookingsRepo.Verify(repo => repo.CreateAsync(It.IsAny<Booking>()), Times.Once);
        }

        [Fact]
        public async Task IsAvailable_ShouldThrowApiException_WhenNoRoomsAreAddedForCategory()
        {
            // Arrange
            var categoryId = "valid-category-id";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(1);
            _mockRoomsRepo.Setup(repo => repo.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync(new List<Room>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _bookingsService.IsAvailable(categoryId, startDate, endDate));
            Assert.Equal(HttpStatusCode.UnprocessableEntity, exception.StatusCode);
            Assert.Equal("No rooms added for the category", exception.Message);
        }

        [Fact]
        public async Task IsAvailable_ShouldReturnFalse_WhenNoRoomsAreAvailable()
        {
            // Arrange
            var categoryId = "valid-category-id";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(1);
            _mockRoomsRepo.Setup(repo => repo.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync(new List<Room> { new Room { CategoryId = "valid-category-id" } });
            _mockBookingsRepo.Setup(repo => repo.GetBookingsForRange(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Booking> { new Booking { CategoryId = "valid-category-id" } });

            // Act
            var result = await _bookingsService.IsAvailable(categoryId, startDate, endDate);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsAvailable_ShouldReturnTrue_WhenRoomsAreAvailable()
        {
            // Arrange
            var categoryId = "valid-category-id";
            var startDate = DateTime.Now;
            var endDate = DateTime.Now.AddDays(1);
            var rooms = new List<Room> { new Room { Id = "room1" } };
            _mockRoomsRepo.Setup(repo => repo.GetByCategoryAsync(It.IsAny<string>())).ReturnsAsync(rooms);
            _mockBookingsRepo.Setup(repo => repo.GetBookingsForRange(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(new List<Booking>());

            // Act
            var result = await _bookingsService.IsAvailable(categoryId, startDate, endDate);

            // Assert
            Assert.True(result);
        }
    }
}
