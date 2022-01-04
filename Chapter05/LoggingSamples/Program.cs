using LoggingSamples.Categories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Logging.AddJsonConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/first-log", (ILogger<CategoryFiltered> loggerCategory, ILogger<MyCategoryAlert> loggerAlertCategory) =>
{
    loggerCategory.LogInformation("I'm information {0}", new { MyName = "Description" });
    loggerCategory.LogInformation("I'm information {MyName}", "My Name Value");
    loggerCategory.LogDebug("I'm debug {0}", new { MyName = "Description" });

    loggerAlertCategory.LogInformation("I'm alert - information {0}", new { MyName = "Alert Description" });
    loggerAlertCategory.LogDebug("I'm alert - debug {0}", new { MyName = "Alert Description" });
    return Results.Ok();
})
.WithName("GetFirstLog");
app.Run();

internal record CategoryFiltered();

