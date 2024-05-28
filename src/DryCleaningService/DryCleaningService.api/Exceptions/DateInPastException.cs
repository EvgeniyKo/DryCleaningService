namespace DryCleaningService.api.Exceptions
{
    public class DateInPastException : ApplicationException
    {
        public DateInPastException(string message = "The date is in the past")
            : base(message)
        {
        }
    }
}
