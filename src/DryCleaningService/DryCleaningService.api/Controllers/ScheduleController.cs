using DryCleaningService.api.Abstractions;
using DryCleaningService.api.Controllers.Requests;
using DryCleaningService.api.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace DryCleaningService.api.Controllers
{
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        [HttpPost("day-schedule")]
        public void PostDaySchedule([FromBody] DayScheduleRequest hours)
        {
            var dateRule = new GeneralDateRule(hours.OpeningHour, hours.ClosingHour, true);
            _scheduleService.AddDateRule(dateRule);
        }

        [HttpPost("days")]
        public void PostDays([FromBody] DaysRequest days)
        {
            var dateRule = new WeekDayRule(days.DayOfWeek, days.OpeningHour, days.ClosingHour, true);
            _scheduleService.AddDateRule(dateRule);
        }

        [HttpPost("dates")]
        public void PostDates([FromBody] DatesRequest dates)
        {
            var dateRule = new DateRule(dates.Date, dates.OpeningHour, dates.ClosingHour, true);
            _scheduleService.AddDateRule(dateRule);
        }

        [HttpPost("days-close")]
        public void PostDaysClose([FromBody] DaysCloseRequest days)
        {
            foreach (var day in days.DaysOfWeek)
            {
                var dateRule = new WeekDayRule(day, TimeOnly.MinValue, TimeOnly.MaxValue, false);
                _scheduleService.AddDateRule(dateRule);
            }
        }

        [HttpPost("dates-close")]
        public void PostDatesClose([FromBody] DatesCloseRequest dates)
        {
            foreach (var date in dates.Dates)
            {
                var dateRule = new DateRule(date, TimeOnly.MinValue, TimeOnly.MaxValue, false);
                _scheduleService.AddDateRule(dateRule);
            }
        }

        [HttpGet("schedule-calculator")]
        public ActionResult<string> Get([FromQuery] int minutes, [FromQuery] DateTime date)
        {
            var finishDate = _scheduleService.Calculate(minutes, date);
            return finishDate.ToString("ddd MMM dd HH:mm:ss yyyy");
        }
    }
}
