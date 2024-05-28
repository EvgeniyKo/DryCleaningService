using DryCleaningService.api.Converters;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DaysRequest : DayScheduleRequest
    {
        [JsonConverter(typeof(DayOfWeekConverter))]
        public DayOfWeek DayOfWeek { get; set; } = DayOfWeek.Sunday;
    }
}
