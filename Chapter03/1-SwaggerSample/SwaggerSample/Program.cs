using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = builder.Environment.ApplicationName, Version = "v1" });
    c.EnableAnnotations();
    c.OperationFilter<AnnotationsOperationFilter>();
    //// Add comment
    var xpath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    c.IncludeXmlComments(xpath);   
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
   .Accepts<ResponseData>(MediaTypeNames.Application.Json)
   .WithMetadata(new SwaggerOperationAttribute("My Summary", "My Description"));

app.Run();

internal record ResponseData(string Value);