using DryCleaningService.api.Abstractions;
using DryCleaningService.api.Implementations;
using DryCleaningService.api.Implementations.Providers;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace DryCleaningService.api.Tests
{
    public partial class ScheduleControllerTests : IClassFixture<TestWebApplicationFactory<Program>>, IDisposable
    {
        private readonly TestWebApplicationFactory<Program> _factory;

        public ScheduleControllerTests(TestWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task PostDaySchedule_ValidParameters_Success()
        {
            var response = await PostAsync("/day-schedule", """{ "OpeningHour":"09:00", "ClosingHour":"16:00" }""");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostDays_ValidParameters_Success()
        {
            var response = await PostAsync("/days", """{ "DayOfWeek":"friday",  "OpeningHour":"10:00", "ClosingHour":"15:00" }""");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostDates_ValidParameters_Success()
        {
            var response = await PostAsync("/dates", """{ "Date":"2010-12-24",  "OpeningHour":"08:00", "ClosingHour":"13:00" }""");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostDaysClose_ValidParameters_Success()
        {
            var response = await PostAsync("/days-close", """{ "DaysOfWeek":["Sunday", "Wednesday"] }""");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task PostDatesClose_ValidParameters_Success()
        {
            var response = await PostAsync("/dates-close", """{ "Dates":["2010-12-25", "2010-12-31"] }""");
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

        private async Task<HttpResponseMessage> PostAsync(string uri, string jsonContent)
        {
            using var client = _factory.CreateClient();
            var content = new StringContent(jsonContent, new MediaTypeHeaderValue(MediaTypeNames.Application.Json));
            return await client.PostAsync(uri, content);
        }


    }
}