using DryCleaningService.api.Abstractions;
using DryCleaningService.api.Exceptions;

namespace DryCleaningService.api.Implementations
{
    public class ScheduleService : IScheduleService
    {
        private readonly IDateService _dateTimeService;
        private readonly IEnumerable<IDateRuleProvider> _dateRuleProviders;

        public ScheduleService(IDateService dateTimeService, IEnumerable<IDateRuleProvider> dateRuleProviders)
        {
            _dateTimeService = dateTimeService;
            _dateRuleProviders = dateRuleProviders;
        }

        public void AddDateRule(IDateRule rule)
        {
            var ruleProvider = _dateRuleProviders.First(x => x.Type == rule.Type);
            ruleProvider.AddRule(rule);
        }

        public DateTime Calculate(int minutes, DateTime dateTime)
        {
            // There is no sence to book the place for the date in past
            if (dateTime < _dateTimeService.Current)
                throw new DateInPastException();

            // The service won't be able to calculate the finish date if it's closed.
            var dateOnly = DateOnly.FromDateTime(dateTime);
            if (!_dateRuleProviders.Any(x => x.HasOpenHours(dateOnly)))
                throw new ServiceClosedException();

            // The most specific provider is the fist one and the most general provider is the last one
            var sortedProviders = _dateRuleProviders.ToList();
            sortedProviders.Sort((x, y) => y.Priority - x.Priority);

            // Need to process until the whole time is taken
            while (minutes > 0)
            {
                var currentDateOnly = DateOnly.FromDateTime(dateTime);
                var rule = FindRule(sortedProviders, currentDateOnly);
                if (rule?.IsOpen == true)
                {
                    // if the open span was found need to calculate minutes and update the finish date
                    var requestedTime = TimeOnly.FromDateTime(dateTime);
                    var startTime = rule.OpeningHour > requestedTime ? rule.OpeningHour : requestedTime;
                    var workingSpan = rule.ClosingHour - startTime;
                    var minMinutes = Math.Min(minutes, (int)workingSpan.TotalMinutes);
                    minutes -= minMinutes;
                    dateTime = new DateTime(currentDateOnly, startTime.AddMinutes(minMinutes));
                }

                if (minutes > 0)
                {
                    /*
                     * The most obvious way to keep processing is to inrement day until availalble slot is found,
                     * but it might work pretty slow in some cases.
                     * Let's say the service migth work one day per month or per year.
                     */
                    dateTime = GetNextAvailableDate(sortedProviders, dateTime);
                }
            }

            return dateTime;
        }

        private IDateRule? FindRule(List<IDateRuleProvider> ruleProviders, DateOnly date)
        {
            IDateRule? rule = null;
            for (var i = 0; rule == null && i < ruleProviders.Count; i++)
            {
                var provider = ruleProviders[i];
                rule = provider.FindRule(date);
            }
            return rule;
        }

        private DateTime GetNextAvailableDate(List<IDateRuleProvider> ruleProviders, DateTime dateTime)
        {
            var nextDay = DateOnly.FromDateTime(dateTime.AddDays(1));
            DateTime? result = null;

            foreach (var provider in ruleProviders)
            {
                var closestDate = provider.GetClosesOpenDate(nextDay);
                if (closestDate == null) continue;

                var closestDay = DateOnly.FromDateTime(closestDate.Value);
                if (closestDay == nextDay)
                {
                    result = closestDate.Value;
                    break;
                }
                // searching for the closest possible date
                if (result == null || closestDate.Value.Date > result.Value.Date)
                    result = closestDate;
            }

            return result.HasValue
                ? result.Value
                : throw new ServiceClosedException();
        }
    }
}
