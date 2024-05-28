namespace DryCleaningService.api.Tests
{
    public partial class ScheduleControllerTests
    {
        [Fact]
        public async Task Get_GeneralTimeSlotIsBigEnough_Success()
        {
            await SetupTestData();
            var dateTime = await SuccessfullyCalculateAsync(120, "2010-06-07 09:10");
            Assert.Equal("Mon Jun 07 11:10:00 2010", dateTime);
        }

        [Fact]
        public async Task Get_TimeSlotIsSmallAndHolidays_Success()
        {
            await SetupTestData();
            var dateTime = await SuccessfullyCalculateAsync(15, "2010-06-08 14:48");
            Assert.Equal("Thu Jun 10 09:03:00 2010", dateTime);
        }

        [Fact]
        public async Task Get_StartBeforeOpeningAndHolidays_Success()
        {
            await SetupTestData();
            var dateTime = await SuccessfullyCalculateAsync(420, "2010-12-24 6:45");
            Assert.Equal("Mon Dec 27 11:00:00 2010", dateTime);
        }

        [Fact]
        public async Task Get_NoWorkingSchedule_Error()
        {
            await FailedCalculateAsync(420, "2010-12-24 6:45");
        }

        [Fact]
        public async Task Get_DateInPast_Error()
        {
            await FailedCalculateAsync(10, "2010-01-24 6:45");
        }

        private async Task SetupTestData()
        {
            await PostAsync("/day-schedule", """{ "OpeningHour":"09:00", "ClosingHour":"15:00" }""");
            await PostAsync("/days", """{ "DayOfWeek":"friday",  "OpeningHour":"10:00", "ClosingHour":"17:00" }""");
            await PostAsync("/dates", """{ "Date":"2010-12-24",  "OpeningHour":"08:00", "ClosingHour":"13:00" }""");

            await PostAsync("/days-close", """{ "DaysOfWeek":["Sunday", "Wednesday"] }""");
            await PostAsync("/dates-close", """{ "Dates":["2010-12-25", "2010-12-31"] }""");
        }

        private async Task<HttpResponseMessage> CalculateAsync(int minutes, string date)
        {
            using var client = _factory.CreateClient();
            var uri = $"/schedule-calculator?minutes={minutes}&date={date}";
            var response = await client.GetAsync(uri);
            return response;
        }

        private async Task<string> SuccessfullyCalculateAsync(int minutes, string date)
        {
            var response = await CalculateAsync(minutes, date);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private async Task FailedCalculateAsync(int minutes, string date)
        {
            var response = await CalculateAsync(minutes, date);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
