using System.ComponentModel.DataAnnotations;

namespace HotelManagementSystem.Common.CustomValidators
{
    public class FutureAttribute : ValidationAttribute
    {
        private readonly string _errorMessage;
        public FutureAttribute(string errorMessage)
        {
            _errorMessage = errorMessage;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            return value is DateTime date && date >= DateTime.Today ? ValidationResult.Success : new ValidationResult(_errorMessage);
        }
    }
}
