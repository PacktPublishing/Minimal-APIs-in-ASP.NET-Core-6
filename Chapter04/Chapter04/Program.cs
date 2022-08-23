var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ILogWriter, ConsoleLogWriter>();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", (IWeatherForecastService weatherForecastService) =>
{
    return weatherForecastService.GetForecast();
});

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IWeatherForecastService>();
    service.GetForecast();
}

app.Run();

public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public interface IWeatherForecastService
{
    WeatherForecast[] GetForecast();
}

public class WeatherForecastService : IWeatherForecastService
{
    public WeatherForecast[] GetForecast()
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, 5).Select(index =>
             new WeatherForecast
             (
                 DateTime.Now.AddDays(index),
                 Random.Shared.Next(-20, 55),
                 summaries[Random.Shared.Next(summaries.Length)]
             ))
             .ToArray();
        return forecast;
    }
}


//public class LogWriter
//{
//    public void Log(string message)
//    {
//        Console.WriteLine($"LogWriter.Write(message: \"{message}\")");
//    }
//}


//public class Worker
//{
//    private readonly LogWriter _logWriter = new LogWriter();

//    protected async Task ExecuteAsync(CancellationToken stoppingToken)
//    {
//        while (!stoppingToken.IsCancellationRequested)
//        {
//            _logWriter.Log($"Worker running at: {DateTimeOffset.Now}");
//            await Task.Delay(1000, stoppingToken);
//        }
//    }
//}

public interface ILogWriter
{
    void Log(string message);
}

public class ConsoleLogWriter : ILogWriter
{
    public void Log(string message)
    {
        Console.WriteLine($"ConsoleLogWriter.Write(message: \"{message}\")");
    }
}

public class Worker
{
    private readonly ILogWriter _logWriter;

    public Worker(ILogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logWriter.Log($"Worker running at: {DateTimeOffset.Now}");
            await Task.Delay(1000, stoppingToken);
        }
    }
}