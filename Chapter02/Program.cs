using System.Security.Claims;
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

// This won't compile
//app.MapGet("/people", (int pageIndex = 0, int itemsPerPage = 50) => { });

string SearchMethod(int pageIndex = 0, int itemsPerPage = 50, string? orderBy = null)
    => $"Sample result for page {pageIndex} getting {itemsPerPage} elements (ordered by {orderBy})";

app.MapGet("/people", SearchMethod);

app.MapGet("/products", (HttpContext context, HttpRequest req, HttpResponse res, ClaimsPrincipal user) => { });

app.Run();

internal class PeopleService { }

internal record Person(string FirstName, string LastName);