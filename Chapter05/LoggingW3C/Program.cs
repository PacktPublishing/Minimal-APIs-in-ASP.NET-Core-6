using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddW3CLogging(logging =>
{
    logging.LoggingFields = W3CLoggingFields.All;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseW3CLogging();

app.MapGet("/first-w3c-log", (IWebHostEnvironment webHostEnvironment) =>
{
    return Results.Ok(new { PathToWrite = webHostEnvironment.ContentRootPath });
})
.WithName("GetW3CLog");
app.Run();
