namespace ZipPay.Common.Rest.Models.UserServiceApi
{
    public class UserResponse
    {
        public long Id { get; set; }

        public string EmailAddress { get; set; }

        public string Name { get; set; }

        public decimal MonthlySalary { get; set; }

        public decimal MonthlyExpenses { get; set; }
    }
}
