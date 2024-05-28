using System.Text.Json;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Converters
{
    public class DateOnlyConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateStr = reader.GetString();
            if (string.IsNullOrEmpty(dateStr))
                throw new InvalidOperationException();

            return DateOnly.ParseExact(dateStr, "yyyy-MM-dd");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
