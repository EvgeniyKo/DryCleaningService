using DryCleaningService.api.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DryCleaningService.api.Implementations.Providers
{
    public class DateRuleProvider : IDateRuleProvider
    {
        private class DateRuleComparer : IComparer<DateRule>
        {
            public int Compare(DateRule? x, DateRule? y)
                => x!.Date == y!.Date
                ? 0
                : x.Date > y.Date
                ? 1
                : -1;
        }

        private static readonly DateRuleComparer Comparer = new DateRuleComparer();

        private List<DateRule> _rules = new List<DateRule>();

        public DateRuleType Type => DateRuleType.Date;

        public bool HasOpenHours(DateOnly startDate)
        {
            var index = SearchRuleIndex(startDate);
            if (index < 0)
                index = ~index;

            for (int i = index; i < _rules.Count; i++)
            {
                var rule = _rules[i];
                if (rule.IsOpen)
                    return true;
            }
            return false;
        }

        public void AddRule(IDateRule rule)
        {
            if (rule.Type != DateRuleType.Date || rule is not DateRule dateRule)
                throw new ArgumentException();

            var index = _rules.BinarySearch(dateRule, Comparer);
            if (index >= 0)
                _rules[index] = dateRule;
            else
                _rules.Insert(~index, dateRule);
        }

        public IDateRule? FindRule(DateOnly date)
        {
            var index = SearchRuleIndex(date);
            return index >= 0 && index < _rules.Count
                ? _rules[index] : null;
        }

        public DateTime? GetClosesOpenDate(DateOnly date)
        {
            var index = SearchRuleIndex(date);
            DateRule? rule = null;
            if (index >= 0 && index < _rules.Count)
                rule = _rules[index];
            else if (index < 0 && ~index < _rules.Count)
                rule = _rules[~index];
            return rule != null ? new DateTime(rule.Date, rule.OpeningHour) : null;
        }

        private int SearchRuleIndex(DateOnly date)
        {
            var searchRule = new DateRule(date, TimeOnly.MinValue, TimeOnly.MaxValue, false);
            return _rules.BinarySearch(searchRule, Comparer);
        }
    }
}
