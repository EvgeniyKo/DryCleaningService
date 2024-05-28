namespace DryCleaningService.api.Abstractions
{
    public interface IScheduleService
    {
        void AddDateRule(IDateRule rule);

        DateTime Calculate(int minutes, DateTime dateTime);
    }
}
