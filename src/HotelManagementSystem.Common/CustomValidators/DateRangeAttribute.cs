using HotelManagementSystem.Common.Models.Request;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Common.CustomValidators
{
    public class DateRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var bookingRequest = (BookingRequest)validationContext.ObjectInstance;

            if (bookingRequest.StartDate > bookingRequest.EndDate)
            {
                return new ValidationResult("StartDate must be earlier than EndDate.");
            }

            return ValidationResult.Success;
        }
    }
}
