using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();
Log.Logger = new LoggerConfiguration()
    .WriteTo
    .ApplicationInsights(app.Services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces)
    .CreateLogger();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/serilog", (ILogger<CategoryFiltered> loggerCategory) =>
{
    loggerCategory.LogInformation("I'm information {MyName}", "My Name Information");
    loggerCategory.LogDebug("I'm debug {MyName}", "My Name Debug");
    loggerCategory.LogInformation("I'm debug {Data}", new PayloadData("CategoryRoot", "Debug"));
    var p = new PayloadData("AlertCategory", "Debug");
    loggerCategory.LogDebug("I'm debug {Data}", p);

    loggerCategory.LogInformation("I'm {@Person}", new Person("Andrea", "Tosato", new DateTime(1986, 11, 9)));

    return Results.Ok();
})
.WithName("GetFirstLog");
app.Run();

internal record CategoryFiltered();

internal class PayloadData
{
    public PayloadData(string name, string level)
    {
        Name = name;
        Level = level;
    }
    public string Name { get; set; }
    public string Level { get; set; }
}

internal record Person(string Name, string Surname, DateTime Birthdate);