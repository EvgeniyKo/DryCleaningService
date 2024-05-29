using DryCleaningService.api.Abstractions;
using DryCleaningService.api.Implementations;
using DryCleaningService.api.Implementations.Providers;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Web;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DryCleaningService.api.Tests
{
    public partial class ScheduleControllerTests : IClassFixture<TestWebApplicationFactory<Program>>, IDisposable
    {
        private readonly TestWebApplicationFactory<Program> _factory;
        private record UriParameter(string Name, string Value);

        public ScheduleControllerTests(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PutDaySchedule_ValidParameters_Success()
        {
            var response = await PutAsync(
                "/day-schedule",
                new UriParameter("openingHour", "09:00"),
                new UriParameter("closingHour", "16:00"));
            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("-09:00")]
        [InlineData("09:00:01")]
        [InlineData("25:00")]
        public async Task PutDaySchedule_InvalidParameters_Success(string openingHous)
        {
            var response = await PutAsync(
                "/day-schedule",
                new UriParameter("openingHour", openingHous),
                new UriParameter("closingHour", "16:00"));
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutDays_ValidParameters_Success()
        {
            var response = await PutAsync(
                "/days",
                new UriParameter("dayOfWeek", "friday"),
                new UriParameter("openingHour", "09:00"),
                new UriParameter("closingHour", "16:00"));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PutDates_ValidParameters_Success()
        {
            var response = await PutAsync(
                "/dates",
                new UriParameter("date", "2010-12-24"),
                new UriParameter("openingHour", "08:00"),
                new UriParameter("closingHour", "13:00"));

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("12/24/2010")]
        [InlineData("2010-12-24 10:00")]
        [InlineData("text")]
        public async Task PutDates_InvalidParameters_Success(string date)
        {
            var response = await PutAsync(
                "/dates",
                new UriParameter("date", date),
                new UriParameter("openingHour", "08:00"),
                new UriParameter("closingHour", "13:00"));

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PutDaysClose_ValidParameters_Success()
        {
            var response = await PutAsync(
                "/days-close",
                new UriParameter("daysOfWeek", "Sunday,Wednesday"));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PutDatesClose_ValidParameters_Success()
        {
            var response = await PutAsync(
                "/dates-close",
                new UriParameter("dates", "2010-12-25,2010-12-31"));
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            var flags = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
            var providers = _factory.Services.GetRequiredService<IEnumerable<IDateRuleProvider>>();

            var generalProvider = (GeneralDateRuleProvider)providers.First(x => x.Type == DateRuleType.General);
            typeof(GeneralDateRuleProvider).GetField("_generalRule", flags)!.SetValue(generalProvider, null);

            var dateProvider = (DateRuleProvider)providers.First(x => x.Type == DateRuleType.Date);
            typeof(DateRuleProvider).GetField("_rules", flags)!.SetValue(dateProvider, new List<DateRule>());

            var weekDayProvider = (WeekDayRuleProvider)providers.First(x => x.Type == DateRuleType.WeekDay);
            typeof(WeekDayRuleProvider).GetField("_weekRules", flags)!.SetValue(weekDayProvider, new WeekDayRule?[7]);
        }

        private async Task<HttpResponseMessage> PutAsync(string uri, params UriParameter[] parameters)
        {
            var parametersStr = string.Join("&", parameters.Select(parameter => $"{parameter.Name}={HttpUtility.UrlEncode(parameter.Value)}"));

            using var client = _factory.CreateClient();
            var content = new StringContent("{}", new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
            return await client.PutAsync($"{uri}?{parametersStr}", content);
        }
    }
}