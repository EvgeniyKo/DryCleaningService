namespace DryCleaningService.api.Abstractions
{
    public interface IDateRuleProvider
    {
        int Priority => (int)Type;

        DateRuleType Type { get; }

        /// <summary>
        /// Returns true if the service is closed this day or any day in future
        /// </summary>
        bool HasOpenHours(DateOnly startDate);

        /// <summary>
        /// Adds a new rule
        /// </summary>
        void AddRule(IDateRule rule);

        /// <summary>
        /// Returns a rule that matches the date
        /// </summary>
        IDateRule? FindRule(DateOnly date);

        /// <summary>
        /// Returns the closest day to the provided date when the service works
        /// </summary>
        DateTime? GetClosesOpenDate(DateOnly date);
    }
}
