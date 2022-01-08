using LoggingProvider;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddFile(configuration =>
{
    configuration.PathFolderName = Path.Combine(builder.Environment.ContentRootPath, "logs");
    configuration.IsRollingFile = true;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/provider-log", (ILogger<CategoryFiltered> loggerCategory) =>
{
    loggerCategory.LogInformation("I'm information");
    loggerCategory.LogDebug("I'm debug");
    loggerCategory.LogWarning("I'm warning");
    loggerCategory.LogError("I'm error");
    loggerCategory.LogError(new Exception("My brutal error"), "I'm error");
    return Results.Ok();
})
.WithName("GetProviderLog");
app.Run();

internal record CategoryFiltered();
