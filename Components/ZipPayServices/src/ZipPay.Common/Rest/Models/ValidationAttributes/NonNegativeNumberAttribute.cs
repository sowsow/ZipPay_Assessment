using System.ComponentModel.DataAnnotations;

namespace ZipPay.Common.Rest.Models.ValidationAttributes
{
    public class NonNegativeNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (decimal.TryParse(obj.ToString(), out var value))
            {
                if (value >= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
