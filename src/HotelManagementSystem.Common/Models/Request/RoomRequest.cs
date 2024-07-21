using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Common.Models.Request
{
    public class RoomRequest
    {
        [Required]
        public string CategoryId { get; set; }
        [Required]
        [MinLength(3), MaxLength(50)]
        public string RoomName { get; set; }
        public double SizeInSquareFeet { get; set; }
        public int FloorNumber { get; set; }
    }
}
