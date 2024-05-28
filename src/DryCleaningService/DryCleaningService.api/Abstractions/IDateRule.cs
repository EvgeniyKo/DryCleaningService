namespace DryCleaningService.api.Abstractions
{
    public enum DateRuleType
    {
        General,
        WeekDay,
        Date
    }

    public record TimeSlot(TimeOnly Start, TimeOnly End);

    public interface IDateRule
    {
        DateRuleType Type { get; }

        TimeOnly OpeningHour { get; }

        TimeOnly ClosingHour { get; }

        bool IsOpen { get; }
    }
}
