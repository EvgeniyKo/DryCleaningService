using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Converters
{
    public class DayOfWeekConverter : JsonConverter<DayOfWeek>
    {
        public override DayOfWeek Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dayStr = reader.GetString();
            if (string.IsNullOrEmpty(dayStr)) throw new InvalidOperationException();

            var converter = TypeDescriptor.GetConverter(typeof(DayOfWeek));
            var dayOfWeekObj = converter.ConvertFromString(dayStr);
            if (dayOfWeekObj == null) throw new InvalidOperationException();

            return (DayOfWeek)dayOfWeekObj;
        }

        public override void Write(Utf8JsonWriter writer, DayOfWeek value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
