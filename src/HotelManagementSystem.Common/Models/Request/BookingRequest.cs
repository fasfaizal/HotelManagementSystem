using HotelManagementSystem.Common.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Common.Models.Request
{
    public class BookingRequest
    {
        public string CategoryId { get; set; }
        [DateRange]
        [Future("Start date should be greater than or equal to todays date")]
        public DateTime StartDate { get; set; }
        [Future("End date should be greater than or equal to todays date")]
        public DateTime EndDate { get; set; }
        [Range(1, 20)]
        public int NoOfGuests { get; set; }
    }
}
