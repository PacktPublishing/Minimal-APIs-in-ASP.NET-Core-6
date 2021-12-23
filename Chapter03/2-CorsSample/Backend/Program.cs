using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var corsPolicy = new CorsPolicyBuilder("http://localhost:5200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .Build();
// Default Policy
//builder.Services.AddCors(c => c.AddDefaultPolicy(corsPolicy));

// Custom Policy
builder.Services.AddCors(options => options.AddPolicy("MyCustomPolicy", corsPolicy));

var app = builder.Build();
// Defualt policy
//app.UseCors();

// Custom Policy
app.UseCors("MyCustomPolicy");

app.MapGet("/api/cors", () =>
{
    return Results.Ok(new { CorsResultJson = true });
});

// With Extension
app.MapGet("/api/cors/extension", () =>
{
    return Results.Ok(new { CorsResultJson = true });
})
.RequireCors("MyCustomPolicy");

// With Annotation
app.MapGet("/api/cors/annotation", [EnableCors("MyCustomPolicy")] () =>
{
    return Results.Ok(new { CorsResultJson = true });
});

app.Run();
