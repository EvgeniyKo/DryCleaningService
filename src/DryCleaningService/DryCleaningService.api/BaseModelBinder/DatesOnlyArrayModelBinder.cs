namespace DryCleaningService.api.BaseModelBinder
{
    public class DatesOnlyArrayModelBinder : BaseModelBinder<DateOnly[]>
    {
        protected override bool TryConvertValue(string valueStr, out DateOnly[] value)
        {
            var valueList = new LinkedList<DateOnly>();
            var items = valueStr.Split(',');
            foreach (var item in items)
            {
                if (!DateOnly.TryParseExact(item.Trim(), "yyyy-MM-dd", out var date))
                {
                    value = Array.Empty<DateOnly>();
                    return false;
                }
                valueList.AddLast(date);
            }

            value = valueList.ToArray();
            return true;
        }
    }
}
