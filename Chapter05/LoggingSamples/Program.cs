var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddJsonConsole();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/first-log", (ILogger<CategoryFiltered> loggerCategory) =>
{
    loggerCategory.LogInformation("I'm information {0}", new { MyName = "Description" });
    loggerCategory.LogDebug("I'm debug {0}", new { MyName = "Description" });
    return Results.Ok();
})
.WithName("GetFirstLog");
app.Run();

internal record CategoryFiltered();