using DryCleaningService.api.BaseModelBinder;
using Microsoft.AspNetCore.Mvc;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DaysCloseRequest
    {
        [FromQuery(Name = "daysOfWeek")]
        [ModelBinder(typeof(DayOfWeekArrayModelBinder))]
        public DayOfWeek[] DaysOfWeek { get; set; } = Array.Empty<DayOfWeek>();
    }
}
