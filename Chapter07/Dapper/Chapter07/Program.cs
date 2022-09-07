using Dapper;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IIcecreamsRepository, IcecreamsRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/icecreams", async (IIcecreamsRepository repository, Icecream icecream) =>
{
    await repository.CreateIcecream(icecream);
    return Results.Ok();
});

app.MapGet("/icecreams", async (IIcecreamsRepository repository) => await repository.GetIcecreams());

app.Run();

public class Icecream
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("SqlConnection");
    }
    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}

public interface IIcecreamsRepository
{
    public Task<IEnumerable<Icecream>> GetIcecreams();
    public Task CreateIcecream(Icecream icecream);
}

public class IcecreamsRepository : IIcecreamsRepository
{
    private readonly DapperContext _context;
    public IcecreamsRepository(DapperContext context)
    {
        _context = context;
    }

public async Task CreateIcecream(Icecream icecream)
{
    var query = "INSERT INTO Icecreams (Name, Description) VALUES (@Name, @Description)";
    var parameters = new DynamicParameters();
    parameters.Add("Name", icecream.Name, DbType.String);
    parameters.Add("Description", icecream.Description, DbType.String);
    using (var connection = _context.CreateConnection())
    {
        await connection.ExecuteAsync(query, parameters);
    }
}

    public async Task<IEnumerable<Icecream>> GetIcecreams()
    {
        var query = "SELECT * FROM Icecreams";
        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<Icecream>(query);
            return result.ToList();
        }
    }
}