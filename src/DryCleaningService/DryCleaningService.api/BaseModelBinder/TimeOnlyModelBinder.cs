namespace DryCleaningService.api.BaseModelBinder
{
    public class TimeOnlyModelBinder : BaseModelBinder<TimeOnly>
    {
        protected override bool TryConvertValue(string valueStr, out TimeOnly value)
            => TimeOnly.TryParseExact(valueStr, "HH:mm", out value);
    }
}
