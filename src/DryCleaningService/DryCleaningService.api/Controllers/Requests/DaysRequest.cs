using DryCleaningService.api.BaseModelBinder;
using Microsoft.AspNetCore.Mvc;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DaysRequest : DayScheduleRequest
    {
        [FromQuery(Name = "dayOfWeek")]
        [ModelBinder(typeof(DayOfWeekModelBinder))]
        public DayOfWeek DayOfWeek { get; set; } = DayOfWeek.Sunday;
    }
}
