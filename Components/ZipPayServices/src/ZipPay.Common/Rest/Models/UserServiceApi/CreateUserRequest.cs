using System.ComponentModel.DataAnnotations;
using ZipPay.Common.Rest.Models.ValidationAttributes;

namespace ZipPay.Common.Rest.Models.UserServiceApi
{
    public class CreateUserRequest
    {
        public long Id { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EmailAddress { get; set; }

        public string Name { get; set; }

        [Required]
        [NonNegativeNumber(ErrorMessage = "Monthly salary must be a positive number")]
        public decimal MonthlySalary { get; set; }

        [Required]
        [NonNegativeNumber(ErrorMessage = "Monthly expenses must be a positive number")]
        public decimal MonthlyExpenses { get; set; }
    }
}
