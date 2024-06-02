namespace DryCleaningService.api.BaseModelBinder
{
    public class DayOfWeekModelBinder : BaseModelBinder<DayOfWeek>
    {
        protected override bool TryConvertValue(string valueStr, out DayOfWeek value)
        {
            var trimmedValue = valueStr.Trim();
            return Enum.TryParse(trimmedValue, true, out value)
                && !int.TryParse(trimmedValue, out var _)
                && Enum.IsDefined(value);
        }
    }
}
