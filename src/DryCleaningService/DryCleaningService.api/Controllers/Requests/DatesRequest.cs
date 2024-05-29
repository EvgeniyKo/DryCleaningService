using DryCleaningService.api.BaseModelBinder;
using Microsoft.AspNetCore.Mvc;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DatesRequest : DayScheduleRequest
    {
        [FromQuery(Name = "date")]
        [ModelBinder(typeof(DateOnlyModelBinder))]
        public DateOnly Date { get; set; } = DateOnly.MinValue;
    }
}
