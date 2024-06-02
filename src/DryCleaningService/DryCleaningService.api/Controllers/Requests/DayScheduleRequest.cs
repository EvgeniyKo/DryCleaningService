using DryCleaningService.api.BaseModelBinder;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DryCleaningService.api.Controllers.Requests
{
    public class DayScheduleRequest : IValidatableObject
    {
        [FromQuery(Name = "openingHour")]
        [ModelBinder(typeof(TimeOnlyModelBinder))]
        public TimeOnly OpeningHour { get; set; } = TimeOnly.MinValue;

        [FromQuery(Name = "closingHour")]
        [ModelBinder(typeof(TimeOnlyModelBinder))]
        public TimeOnly ClosingHour { get; set; } = TimeOnly.MinValue;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = OpeningHour < ClosingHour
                ? ValidationResult.Success!
                : new ValidationResult("Service should open before it's closed", [nameof(OpeningHour), nameof(ClosingHour)]);
            yield return result;
        }
    }
}
