using DryCleaningService.api.Abstractions;

namespace DryCleaningService.api.Implementations.Providers
{
    public class WeekDayRuleProvider : IDateRuleProvider
    {
        private WeekDayRule?[] _weekRules = new WeekDayRule?[7];
        private bool _hasOpenHours = false;

        public DateRuleType Type => DateRuleType.WeekDay;

        public bool HasOpenHours(DateOnly startDate)
            => _hasOpenHours;

        public void AddRule(IDateRule rule)
        {
            if (rule.Type != DateRuleType.WeekDay || rule is not WeekDayRule weekDayRule)
                throw new ArgumentException();

            _weekRules[(int)weekDayRule.DayOfWeek] = weekDayRule;
            _hasOpenHours = _weekRules.Any(x => x?.IsOpen == true);
        }

        public IDateRule? FindRule(DateOnly date)
            => _weekRules[(int)date.DayOfWeek];

        public DateTime? GetClosesOpenDate(DateOnly date)
        {
            var index = (int)date.DayOfWeek;
            WeekDayRule? rule = null;
            int i = 0;
            for (; rule == null && i < _weekRules.Length; i++)
            {
                var currentDayIndex = (index + i) % _weekRules.Length;
                var currentRule = _weekRules[currentDayIndex];
                if (currentRule?.IsOpen == true)
                    rule = currentRule;
            }

            return rule != null
                ? new DateTime(date.AddDays(i), rule.OpeningHour)
                : null;
        }


    }
}
