namespace DryCleaningService.api.Converters
{
    public class DateOnlyArrayConverter : JsonArrayConverter<DateOnly>
    {
        protected override IEnumerable<DateOnly> GetItems(IEnumerable<string> values)
            => values.Select(x => DateOnly.ParseExact(x, "yyyy-MM-dd"));
    }
}
