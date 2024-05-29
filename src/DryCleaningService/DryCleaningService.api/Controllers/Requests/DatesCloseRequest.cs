using DryCleaningService.api.BaseModelBinder;
using Microsoft.AspNetCore.Mvc;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DatesCloseRequest
    {
        [FromQuery(Name = "dates")]
        [ModelBinder(typeof(DatesOnlyArrayModelBinder))]
        public DateOnly[] Dates { get; set; } = Array.Empty<DateOnly>();
    }
}
