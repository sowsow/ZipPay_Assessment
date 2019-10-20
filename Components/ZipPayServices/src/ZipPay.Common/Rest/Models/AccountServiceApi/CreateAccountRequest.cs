using System.ComponentModel.DataAnnotations;

namespace ZipPay.Common.Rest.Models.AccountServiceApi
{
    public class CreateAccountRequest
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [Required]
        public long CreatedByUserId { get; set; }
    }
}
