using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<PeopleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Chapter 2 API");
});

app.MapPut("/people/{id:int}",
    (int id, bool notify, Person person, PeopleService service) => { });

app.MapGet("/search", ([FromQuery(Name = "q")] string searchText) => { });

app.Run();

internal class PeopleService { }

internal record Person(string FirstName, string LastName);