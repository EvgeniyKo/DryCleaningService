using System.Text.Json;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Converters
{
    public abstract class JsonArrayConverter<T> : JsonConverter<T[]>
    {
        public override T[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringArray = ReadStrings(ref reader);
            var filteredArray = stringArray
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x));

            var itemList = GetItems(filteredArray).ToArray();
            return itemList.Length > 0 ? itemList : null;
        }

        public override void Write(Utf8JsonWriter writer, T[] value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        protected abstract IEnumerable<T> GetItems(IEnumerable<string> values);

        private IEnumerable<string> ReadStrings(ref Utf8JsonReader reader)
        {
            /* In such case it's better to use yield return,
             * which makes compiler to generate state machine,
             * but it's imposible because of Utf8JsonReader
             */

            var result = new LinkedList<string>();
            if (reader.TokenType == JsonTokenType.StartArray)
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.String)
                    {
                        var dayOfWeekStr = reader.GetString();
                        if (!string.IsNullOrWhiteSpace(dayOfWeekStr))
                            result.AddLast(dayOfWeekStr);
                    }
                    else if (reader.TokenType == JsonTokenType.EndArray)
                        break;
                }
            }
            return result;
        }
    }
}
