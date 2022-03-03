using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Chapter09.Swagger;
using Microsoft.AspNetCore.Localization;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AcceptLanguageHeaderOperationFilter>();
});

var supportedCultures = new CultureInfo[] { new("en"), new("it"), new("fr") };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.DefaultRequestCulture = new RequestCulture(supportedCultures.First());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRequestLocalization();

app.MapGet("/api/culture", () => Thread.CurrentThread.CurrentCulture.DisplayName);

app.MapGet("/api/helloworld", () => Chapter09.Resources.Messages.HelloWorld);

app.MapGet("/api/hello", (string name) =>
{
    var message = string.Format(Chapter09.Resources.Messages.GreetingMessage, name);
    return message;
});

app.MapPost("/people-dataannotations", (AnnotatedPerson person) =>
{
    var isValid = MiniValidator.TryValidate(person, out var errors);
    if (!isValid)
    {
        return Results.ValidationProblem(errors);
    }

    return Results.NoContent();
})
.Produces(StatusCodes.Status204NoContent)
.ProducesValidationProblem();

app.Run();

public class AnnotatedPerson
{
    [Display(Name = "FirstName", ResourceType = typeof(Chapter09.Resources.Messages))]
    [Required(ErrorMessageResourceName = "FieldRequired",
        ErrorMessageResourceType = typeof(Chapter09.Resources.Messages))]
    [MaxLength(30, ErrorMessageResourceName = "MaxLength",
        ErrorMessageResourceType = typeof(Chapter09.Resources.Messages))]
    public string FirstName { get; set; }

    [Display(Name = "LastName", ResourceType = typeof(Chapter09.Resources.Messages))]
    [Required(ErrorMessageResourceName = "FieldRequired",
        ErrorMessageResourceType = typeof(Chapter09.Resources.Messages))]
    [MaxLength(30, ErrorMessageResourceName = "MaxLength",
        ErrorMessageResourceType = typeof(Chapter09.Resources.Messages))]
    public string LastName { get; set; }

    [EmailAddress(ErrorMessageResourceName = "InvalidField",
        ErrorMessageResourceType = typeof(Chapter09.Resources.Messages))]
    [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "StringLength",
        ErrorMessageResourceType = typeof(Chapter09.Resources.Messages))]
    public string Email { get; set; }
}