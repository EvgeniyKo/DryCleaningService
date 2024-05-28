namespace DryCleaningService.api.Exceptions
{
    public class ServiceClosedException : ApplicationException
    {
        public ServiceClosedException(string message = "Service is closed")
            : base(message)
        {
        }
    }
}
