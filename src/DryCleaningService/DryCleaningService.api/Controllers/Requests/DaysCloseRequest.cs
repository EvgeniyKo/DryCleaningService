using DryCleaningService.api.Converters;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DaysCloseRequest
    {
        [JsonConverter(typeof(DayOfWeekArrayConverter))]
        public DayOfWeek[] DaysOfWeek { get; set; } = Array.Empty<DayOfWeek>();
    }
}
