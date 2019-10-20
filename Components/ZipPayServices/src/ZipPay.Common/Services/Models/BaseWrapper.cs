namespace ZipPay.Common.Services.Models
{
    public abstract class BaseWrapper<T> 
    {
        public bool Success { get; private set; }

        public string Message { get; private set; }

        public T Resource { get; set; }

        protected BaseWrapper(T resource)
        {
            Success = true;
            Message = string.Empty;
            Resource = resource;
        }

        protected BaseWrapper(string message)
        {
            Success = false;
            Message = message;
            Resource = default;
        }
    }
}
