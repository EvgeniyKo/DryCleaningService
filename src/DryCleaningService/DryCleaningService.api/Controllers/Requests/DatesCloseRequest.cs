using DryCleaningService.api.Converters;
using System.Text.Json.Serialization;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DatesCloseRequest
    {
        [JsonConverter(typeof(DateOnlyArrayConverter))]
        public DateOnly[] Dates { get; set; } = Array.Empty<DateOnly>();
    }
}
