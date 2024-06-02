using DryCleaningService.api.Abstractions;
using DryCleaningService.api.Exceptions;
using DryCleaningService.api.Implementations;
using DryCleaningService.api.Implementations.Providers;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.MapType<TimeOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = typeof(string).Name });
    s.MapType<DateOnly>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = typeof(string).Name });
    s.MapType<DayOfWeek>(() => new Microsoft.OpenApi.Models.OpenApiSchema { Type = typeof(string).Name });
});

builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddSingleton<IDateService, DateService>();

builder.Services.AddSingleton<IDateRuleProvider, GeneralDateRuleProvider>();
builder.Services.AddSingleton<IDateRuleProvider, WeekDayRuleProvider>();
builder.Services.AddSingleton<IDateRuleProvider, DateRuleProvider>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex) when (ex is ServiceClosedException || ex is DateInPastException)
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }