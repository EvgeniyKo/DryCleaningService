namespace DryCleaningService.api.BaseModelBinder
{
    public class DayOfWeekArrayModelBinder : BaseModelBinder<DayOfWeek[]>
    {
        protected override bool TryConvertValue(string valueStr, out DayOfWeek[] value)
        {
            var valueList = new LinkedList<DayOfWeek>();
            var items = valueStr.Split(',');
            foreach (var item in items)
            {
                if (!Enum.TryParse(item.Trim(), true, out DayOfWeek dayOfWeek))
                {
                    value = Array.Empty<DayOfWeek>();
                    return false;
                }
                valueList.AddLast(dayOfWeek);
            }

            value = valueList.ToArray();
            return true;
        }
    }
}
