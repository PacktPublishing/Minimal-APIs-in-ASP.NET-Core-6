using SwaggerSample.Filter;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = builder.Environment.ApplicationName,
        Version = "v1",
        Contact = new() { Name = "PacktAuthor", Email = "authors@packtpub.com", Url = new Uri("https://www.packtpub.com/") },
        Description = "PacktPub Minimal API - Swagger",
        License = new Microsoft.OpenApi.Models.OpenApiLicense(),
        TermsOfService = new("https://www.packtpub.com/")
    });
    c.OperationFilter<CorrelationIdOperationFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));
}

app.UseHttpsRedirection();

app.MapGet("/sampleresponse", () =>
    {
        return Results.Ok(new ResponseData("My Response"));
    })
    .Produces<ResponseData>(StatusCodes.Status200OK)
    .WithTags("Sample")
    .WithName("SampleResponseOperation"); // operation ids to Open API

app.MapGet("/sampleresponseskipped", () =>
{
    return Results.Ok(new ResponseData("My Response Skipped"));
})
    .ExcludeFromDescription();

app.MapGet("/{id}", (int id) => Results.Ok(id));
app.MapPost("/", (ResponseData data) => Results.Ok(data))
   .Accepts<ResponseData>(MediaTypeNames.Application.Json);

app.Run();

internal record ResponseData(string Value);