using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining<Program>();
});

builder.Services.AddFluentValidationRulesToSwagger();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

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

app.MapPost("/people-fluentvalidation", (Person person, IValidator<Person> validator) =>
{
    var validationResult = validator.Validate(person);
    if (!validationResult.IsValid)
    {
        var errors = validationResult.Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(k => k.Key, v => v.Select(e => e.ErrorMessage).ToArray());

        return Results.ValidationProblem(errors);
    }

    return Results.NoContent();
})
.Produces(StatusCodes.Status204NoContent)
.ProducesValidationProblem();

app.Run();

public class AnnotatedPerson
{
    [Required]
    [MaxLength(30)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(30)]
    public string LastName { get; set; }

    [EmailAddress]
    [StringLength(100, MinimumLength = 6)]
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
        RuleFor(p => p.FirstName).NotEmpty().MaximumLength(30);
        RuleFor(p => p.LastName).NotEmpty().MaximumLength(30);
        RuleFor(p => p.Email).EmailAddress().Length(6, 100);
    }
}