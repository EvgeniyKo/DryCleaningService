using DryCleaningService.api.Converters;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DayScheduleRequest
    {
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly OpeningHour { get; set; } = TimeOnly.MinValue;
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly ClosingHour { get; set; } = TimeOnly.MinValue;
    }
}
