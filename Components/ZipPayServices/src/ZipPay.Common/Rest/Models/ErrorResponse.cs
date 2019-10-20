using System.Collections.Generic;

namespace ZipPay.Common.Rest.Models
{
    public class ErrorResponse
    {
        public bool Success => false;

        public List<string> Messages { get; private set; }

        public ErrorResponse(List<string> messages)
        {
            Messages = messages ?? new List<string>();
        }

        public ErrorResponse(string message)
        {
            Messages = new List<string>();

            if (!string.IsNullOrWhiteSpace(message))
            {
                Messages.Add(message);
            }
        }
    }
}
