using Microsoft.AspNetCore.Mvc;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DaysRequest : DayScheduleRequest
    {
        [FromQuery(Name = "dayOfWeek")]
        public DayOfWeek DayOfWeek { get; set; } = DayOfWeek.Sunday;
    }
}
