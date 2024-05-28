using DryCleaningService.api.Abstractions;

namespace DryCleaningService.api.Implementations
{
    public record GeneralDateRule(TimeOnly OpeningHour, TimeOnly ClosingHour, bool IsOpen) : IDateRule
    {
        public virtual DateRuleType Type => DateRuleType.General;
    }

    public record WeekDayRule(DayOfWeek DayOfWeek, TimeOnly OpeningHour, TimeOnly ClosingHour, bool IsOpen)
        : GeneralDateRule(OpeningHour, ClosingHour, IsOpen)
    {
        public override DateRuleType Type => DateRuleType.WeekDay;
    }

    public record DateRule(DateOnly Date, TimeOnly OpeningHour, TimeOnly ClosingHour, bool IsOpen)
    : GeneralDateRule(OpeningHour, ClosingHour, IsOpen)
    {
        public override DateRuleType Type => DateRuleType.Date;
    }
}
