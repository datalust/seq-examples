using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

ActivitySource source = new("MyApp.Source");

app.MapGet("/weatherforecast", () =>
{
    var min = -20;
    var max = 55;
    
    using var activity = source.StartActivity("Calculating weather with temperature between {Min} and {Max}");
    
    activity?.SetTag("Min", min);
    activity?.SetTag("Max", max);

    var summary = summaries[Random.Shared.Next(summaries.Length)];

    activity?.SetTag("Summary", summary);
    
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(min, max),
            summary
        ))
        .ToArray();
    
    return forecast;
});

app.MapDefaultEndpoints();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
