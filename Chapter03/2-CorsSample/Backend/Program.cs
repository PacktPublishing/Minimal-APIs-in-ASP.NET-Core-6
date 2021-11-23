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

app.Run();
