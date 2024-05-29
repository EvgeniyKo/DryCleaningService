namespace DryCleaningService.api.BaseModelBinder
{
    public class DateOnlyModelBinder : BaseModelBinder<DateOnly>
    {
        protected override bool TryConvertValue(string valueStr, out DateOnly value)
            => DateOnly.TryParseExact(valueStr, "yyyy-MM-dd", out value);
    }
}
