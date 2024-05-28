using DryCleaningService.api.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace DryCleaningService.api.Tests
{
    public class TestWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.First(x => x.ServiceType == typeof(IDateService));
                services.Remove(descriptor);

                services.AddSingleton<IDateService, DateServiceMock>();
            });
        }
    }

    public class DateServiceMock : IDateService
    {
        public DateTime Current => Constants.CurrentDate;
    }
}
