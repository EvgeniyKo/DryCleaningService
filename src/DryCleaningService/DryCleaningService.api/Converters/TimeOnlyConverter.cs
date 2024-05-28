using System.Text.Json;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Converters
{
    public class TimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var timeStr = reader.GetString()!;
            return TimeOnly.ParseExact(timeStr, "HH:mm");
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
