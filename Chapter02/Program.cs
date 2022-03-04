using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Chapter02.Extensions;
using Chapter02.Routing;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.IgnoreReadOnlyProperties = true;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<PeopleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
// Uncomment the TryParse method and comment the BindAsync method on the Location class to make this example work.
app.MapGet("/navigate", (Location location) => $"Location: {location.Latitude}, {location.Longitude}");

// POST /navigate?lat=43.8427&lon=7.8527
// Uncomment the BindAsync method and comment the TryParse method on the Location class to make this example work.
app.MapPost("/navigate", (Location location) => $"Location: {location.Latitude}, {location.Longitude}");

app.MapGet("/ok", () => Results.Ok(new Person("Donald", "Duck")));

app.MapGet("/notfound", () => Results.NotFound());

app.MapPost("/badrequest", () =>
{
    // Creates a 400 response with a JSON body.
    return Results.BadRequest(new { ErrorMessage = "Unable to complete the request" });
});

app.MapGet("/download", (string fileName) => Results.File(fileName));

app.MapGet("/xml", () => Results.Extensions.Xml(new City { Name = "Taggia" }));

app.MapGet("/product", () =>
{
    var product = new Product("Apple", null, 0.42, 6);
    return Results.Ok(product);
});

// This method automatically registers all the handlers that implement the IEndpointRouteHandler interface.
app.MapEndpoints(Assembly.GetExecutingAssembly());

app.Run();

public class PeopleService
{
}

public record class Person(string FirstName, string LastName);

public record class City
{
    public string? Name { get; init; }
}

public record class Product(string Name, string? Description, double UnitPrice, int Quantity)
{
    public double TotalPrice => UnitPrice * Quantity;
}

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

        location = null;
        return false;
    }

    // We can use also
    // public static bool BindAsync(HttpContext content)
    // if we don't need the ParameterInfo
    public static ValueTask<Location?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (double.TryParse(context.Request.Query["lat"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var latitude)
            && double.TryParse(context.Request.Query["lon"], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var longitude))
        {
            var location = new Location { Latitude = latitude, Longitude = longitude };
            return ValueTask.FromResult<Location?>(location);
        }

        return ValueTask.FromResult<Location?>(null);
    }
}