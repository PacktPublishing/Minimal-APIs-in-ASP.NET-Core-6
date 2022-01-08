using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = Application.Json;
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>()!;
        var errorMessage = new
        {
            Message = exceptionHandlerPathFeature.Error.Message
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorMessage));
        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
        {
            await context.Response.WriteAsync(" The file was not found.");
        }
        if (exceptionHandlerPathFeature?.Path == "/")
        {
            await context.Response.WriteAsync("Page: Home.");
        }
    });
});

app.MapGet("/ok-result", () =>
{
    throw new ArgumentNullException("taggia-parameter", "Taggia has an error");
})
.WithName("OkResult");

app.Run();