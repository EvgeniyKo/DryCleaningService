using DryCleaningService.api.Converters;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DatesRequest : DayScheduleRequest
    {
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateOnly Date { get; set; } = DateOnly.MinValue;
    }
}
