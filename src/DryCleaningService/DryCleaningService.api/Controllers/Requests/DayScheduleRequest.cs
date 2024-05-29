using DryCleaningService.api.BaseModelBinder;
using Microsoft.AspNetCore.Mvc;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DayScheduleRequest
    {
        [FromQuery(Name = "openingHour")]
        [ModelBinder(typeof(TimeOnlyModelBinder))]
        public TimeOnly OpeningHour { get; set; } = TimeOnly.MinValue;

        [FromQuery(Name = "closingHour")]
        [ModelBinder(typeof(TimeOnlyModelBinder))]
        public TimeOnly ClosingHour { get; set; } = TimeOnly.MinValue;
    }
}
