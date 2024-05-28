using DryCleaningService.api.Abstractions;

namespace DryCleaningService.api.Implementations.Providers
{
    public class GeneralDateRuleProvider : IDateRuleProvider
    {
        private IDateRule? _generalRule = null;

        public DateRuleType Type => DateRuleType.General;

        public bool HasOpenHours(DateOnly startDate)
            => _generalRule?.IsOpen == true;

        public void AddRule(IDateRule rule)
        {
            if (rule.Type != DateRuleType.General) throw new ArgumentException();

            // Only one general rule can be available in the system
            _generalRule = rule;
        }

        public IDateRule? FindRule(DateOnly date)
            => _generalRule;

        public DateTime? GetClosesOpenDate(DateOnly date)
            => HasOpenHours(date)
            ? new DateTime(date, _generalRule!.OpeningHour)
            : null;


    }
}
