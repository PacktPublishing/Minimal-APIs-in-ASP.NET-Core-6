using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Chapter06.Dtos;
using Chapter06.Entities;
using Chapter06.Extensions;
using FluentValidation;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddFluentValidationRulesToSwagger();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
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

app.MapPost("/people-fluentvalidation", async (Person person, IValidator<Person> validator) =>
{
    var validationResult = await validator.ValidateAsync(person);
    if (!validationResult.IsValid)
    {
        var errors = validationResult.ToDictionary();
        return Results.ValidationProblem(errors);
    }

    return Results.NoContent();
})
.Produces(StatusCodes.Status204NoContent)
.ProducesValidationProblem();

app.MapGet("/people-nomapping/{id:int}", (int id) =>
{
    // In a real application, this entity could be
    // retrieved from a database, checking if the person
    // with the given ID exists.
    var person = new PersonEntity();
    return Results.Ok(person);
})
.Produces(StatusCodes.Status200OK, typeof(PersonEntity))
.Produces(StatusCodes.Status404NotFound);

app.MapGet("/people-manualmapping/{id:int}", (int id) =>
{
    // In a real application, this entity could be
    // retrieved from a database, checking if the person
    // with the given ID exists.
    var personEntity = new PersonEntity
    {
        Id = 42,
        FirstName = "Donald",
        LastName = "Duck",
        BirthDate = new DateTime(1934, 6, 9),
        Address = new AddressEntity
        {
            Street = "1313 Webfoot Street",
            City = "Duckburg"
        }
    };

    var personDto = personEntity.ToDto();
    return Results.Ok(personDto);
})
.Produces(StatusCodes.Status200OK, typeof(PersonDto))
.Produces(StatusCodes.Status404NotFound);

app.MapGet("/people-automapper/{id:int}", (int id, IMapper mapper) =>
{
    // In a real application, this entity could be
    // retrieved from a database, checking if the person
    // with the given ID exists.
    var personEntity = new PersonEntity
    {
        Id = 42,
        FirstName = "Donald",
        LastName = "Duck",
        BirthDate = new DateTime(1934, 6, 9),
        Address = new AddressEntity
        {
            Street = "1313 Webfoot Street",
            City = "Duckburg"
        }
    };

    var personDto = mapper.Map<PersonDto>(personEntity);
    return Results.Ok(personDto);
})
.Produces(StatusCodes.Status200OK, typeof(PersonDto))
.Produces(StatusCodes.Status404NotFound);

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
        _ = RuleFor(p => p.FirstName).NotEmpty()
            .WithMessage("You must provide the first name")
            .MaximumLength(30);

        _ = RuleFor(p => p.LastName).NotEmpty().MaximumLength(30);
        _ = RuleFor(p => p.Email).EmailAddress().Length(6, 100);
    }
}