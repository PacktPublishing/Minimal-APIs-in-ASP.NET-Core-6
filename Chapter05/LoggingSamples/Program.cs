using LoggingSamples.Categories;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddJsonConsole(options =>
        options.JsonWriterOptions = new JsonWriterOptions()
        {
            Indented = true
        });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/first-log", (ILogger<CategoryFiltered> loggerCategory, ILogger<MyCategoryAlert> loggerAlertCategory) =>
{
    loggerCategory.LogInformation("I'm information {MyName}", "My Name Information");
    loggerCategory.LogDebug("I'm debug {MyName}", "My Name Debug");
    loggerCategory.LogInformation("I'm debug {Data}", new PayloadData("CategoryRoot", "Debug"));

    loggerAlertCategory.LogInformation("I'm information {MyName}", "Alert Information");
    loggerAlertCategory.LogDebug("I'm debug {MyName}", "Alert Debug");
    var p = new PayloadData("AlertCategory", "Debug");
    loggerAlertCategory.LogDebug("I'm debug {Data}", p);

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
