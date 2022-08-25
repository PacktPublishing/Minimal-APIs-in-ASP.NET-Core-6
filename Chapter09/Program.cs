using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Chapter09.Resources;
using Chapter09.Serialization;
using Chapter09.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Localization;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new UtcDateTimeConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AcceptLanguageHeaderOperationFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

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

app.MapGet("/culture", () => Thread.CurrentThread.CurrentCulture.DisplayName);

app.MapGet("/helloworld", () => Messages.HelloWorld);

app.MapGet("/hello", (string name) =>
{
    var message = string.Format(Messages.GreetingMessage, name);
    return message;
});

app.MapPost("/people-dataannotations", (AnnotatedPerson person) =>
{
    var isValid = MiniValidator.TryValidate(person, out var errors);
    if (!isValid)
    {
        return Results.ValidationProblem(errors, title: Messages.ValidationErrors);
    }

    return Results.NoContent();
})
.Produces(StatusCodes.Status204NoContent)
.ProducesValidationProblem();

app.MapPost("/people-fluentvalidation", (Person person, IValidator<Person> validator) =>
{
    var validationResult = validator.Validate(person);
    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(k => k.Key, v => v.Select(e => e.ErrorMessage).ToArray());

        return Results.ValidationProblem(errors, title: Messages.ValidationErrors);
    }

    return Results.NoContent();
})
.Produces(StatusCodes.Status204NoContent)
.ProducesValidationProblem();

app.MapPost("/date", (DateInput date) =>
{
    return Results.Ok(new
    {
        Input = date.Value,
        DateKind = date.Value.Kind.ToString(),
        ServerDate = DateTime.Now
    });
});

app.Run();

public class AnnotatedPerson
{
    [Display(Name = "FirstName", ResourceType = typeof(Messages))]
    [Required(ErrorMessageResourceName = "FieldRequiredAnnotation",
        ErrorMessageResourceType = typeof(Messages))]
    [MaxLength(30, ErrorMessageResourceName = "MaxLength",
        ErrorMessageResourceType = typeof(Messages))]
    public string FirstName { get; set; }

    [Display(Name = "LastName", ResourceType = typeof(Messages))]
    [Required(ErrorMessageResourceName = "FieldRequiredAnnotation",
        ErrorMessageResourceType = typeof(Messages))]
    [MaxLength(30, ErrorMessageResourceName = "MaxLengthAnnotation",
        ErrorMessageResourceType = typeof(Messages))]
    public string LastName { get; set; }

    [EmailAddress(ErrorMessageResourceName = "InvalidFieldAnnotation",
        ErrorMessageResourceType = typeof(Messages))]
    [StringLength(100, MinimumLength = 6, ErrorMessageResourceName = "StringLengthAnnotation",
        ErrorMessageResourceType = typeof(Messages))]
    public string Email { get; set; }
}

public class Person
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }
}

public class PersonValidator : AbstractValidator<Person>
{
    public PersonValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage(Messages.NotEmptyMessage)
            .MaximumLength(30).WithMessage(Messages.MaximumLengthMessage)
            .WithName(Messages.FirstName);

        RuleFor(p => p.LastName).NotEmpty().WithMessage(Messages.NotEmptyMessage)
            .MaximumLength(30).WithMessage(Messages.MaximumLengthMessage)
            .WithName(Messages.LastName);

        RuleFor(p => p.Email).EmailAddress().WithMessage(Messages.InvalidFieldMessage)
            .Length(6, 100).WithMessage(Messages.LengthMessage)
            .WithName(Messages.Email);
    }
}

public record DateInput(DateTime Value);
