using System.Globalization;
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

// GET /navigate?location=43.8427,7.8527
app.MapGet("/navigate", (Location location) => $"Location: {location.Latitude}, {location.Longitude}");

app.Run();

internal class PeopleService { }

internal record Person(string FirstName, string LastName);

public class Location
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    // We can use also
    // public static bool TryParse(string? value, out Location? location)
    // if we don't need the FormatProvider
    public static bool TryParse(string? value, IFormatProvider? provider, out Location? location)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            var values = value.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (values.Length == 2
                && double.TryParse(values[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var latitude)
                && double.TryParse(values[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var longitude))
            {
                location = new Location { Latitude = latitude, Longitude = longitude };
                return true;
            }
        }

        throw new BadHttpRequestException("Unable to bind Location", StatusCodes.Status400BadRequest);
    }
}