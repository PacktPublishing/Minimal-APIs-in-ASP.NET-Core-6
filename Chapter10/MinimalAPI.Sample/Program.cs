var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("text-plain",
    () => Results.Content("response"))
.WithName("GetTextPlain");

app.MapPost("validations",
    (ValidationData validation) => Results.Ok(validation))
.WithName("PostValidationData");

app.MapGet("jsons", () =>
    {
        var response = new[]
        {
            new PersonData { Name = "Andrea", Surname = "Tosato", BirthDate = new DateTime(2022, 01, 01) },
            new PersonData { Name = "Emanuele", Surname = "Bartolesi", BirthDate = new DateTime(2022, 01, 01) },
            new PersonData { Name = "Marco", Surname = "Minerva", BirthDate = new DateTime(2022, 01, 01) }
        };
        return Results.Ok(response);
    })
.WithName("GetJsonData");

app.Run();

public class ValidationData
{
    public int Id { get; set; }

    public string Description { get; set; }
}

public class PersonData
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
}