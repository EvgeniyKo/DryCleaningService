using System;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Converters
{
    public class DayOfWeekArrayConverter : JsonArrayConverter<DayOfWeek>
    {
        private static readonly TypeConverter Converter = TypeDescriptor.GetConverter(typeof(DayOfWeek));

        protected override IEnumerable<DayOfWeek> GetItems(IEnumerable<string> values)
            => values
            .Select(Converter.ConvertFromString)
            .Where(x => x != null)
            .Select(x => (DayOfWeek)x!);
    }
}
