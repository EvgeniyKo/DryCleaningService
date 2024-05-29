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

        [HttpPut("day-schedule")]
        public void PutDaySchedule(DayScheduleRequest request)
        {
            var dateRule = new GeneralDateRule(request.OpeningHour, request.ClosingHour, true);
            _scheduleService.AddDateRule(dateRule);
        }

        [HttpPut("days")]
        public void PutDays(DaysRequest request)
        {
            var dateRule = new WeekDayRule(request.DayOfWeek, request.OpeningHour, request.ClosingHour, true);
            _scheduleService.AddDateRule(dateRule);
        }

        [HttpPut("dates")]
        public void PutDates(DatesRequest request)
        {
            var dateRule = new DateRule(request.Date, request.OpeningHour, request.ClosingHour, true);
            _scheduleService.AddDateRule(dateRule);
        }

        [HttpPut("days-close")]
        public void PutDaysClose(DaysCloseRequest request)
        {
            foreach (var day in request.DaysOfWeek)
            {
                var dateRule = new WeekDayRule(day, TimeOnly.MinValue, TimeOnly.MaxValue, false);
                _scheduleService.AddDateRule(dateRule);
            }
        }

        [HttpPut("dates-close")]
        public void PutDatesClose(DatesCloseRequest request)
        {
            foreach (var date in request.Dates)
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
