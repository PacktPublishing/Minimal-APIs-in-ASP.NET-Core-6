using Chapter07.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IcecreamDb>(options => options.UseInMemoryDatabase("icecreams"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/icecreams", async (IcecreamDb db, Icecream icecream) =>
{
    await db.Icecreams.AddAsync(icecream);
    await db.SaveChangesAsync();
    return Results.Created($"/icecreams/{icecream.Id}", icecream);
});

app.MapGet("/icecreams", async (IcecreamDb db) => await db.Icecreams.ToListAsync());

app.MapGet("/icecreams/{id}", async (IcecreamDb db, int id) => await db.Icecreams.FindAsync(id));

app.MapPut("/icecreams/{id}", async (IcecreamDb db, Icecream updateicecream, int id) =>
{
    var icecream = await db.Icecreams.FindAsync(id);
    if (icecream is null) return Results.NotFound();
    icecream.Name = updateicecream.Name;
    icecream.Description = updateicecream.Description;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/icecreams/{id}", async (IcecreamDb db, int id) =>
{
    var icecream = await db.Icecreams.FindAsync(id);
    if (icecream is null)
    {
        return Results.NotFound();
    }
    db.Icecreams.Remove(icecream);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

class IcecreamDb : DbContext
{
    public IcecreamDb(DbContextOptions options) : base(options) { }
    public DbSet<Icecream> Icecreams { get; set; } = null!;
}