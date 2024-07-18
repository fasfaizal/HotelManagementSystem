using HotelManagementSystem.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Common.Models.Request
{
    public class CategoryRequest
    {
        [Required]
        [MinLength(3), MaxLength(50)]
        public string CategoryName { get; set; }
        public string? Description { get; set; }
        [Required]
        [Range(1, 20)]
        public int Capacity { get; set; }
        [Required]
        [Range(1, 99999)]
        public decimal PricePerNight { get; set; }
        public BedType BedType { get; set; }
    }
}
