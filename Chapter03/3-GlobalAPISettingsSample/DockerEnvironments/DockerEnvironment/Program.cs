// Run this sample with DockerCompose
var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/env-test", (IConfiguration configuration) =>
{
    var rootProperty = configuration.GetValue<string>("RootProperty");
    var sampleVariable = configuration.GetValue<string>("RootSettings:SampleVariable");
    var connectionString = configuration.GetConnectionString("SqlConnection");
    return Results.Ok(new
    {
        RootProperty = rootProperty,
        SampleVariable = sampleVariable,
        ConnetionString = connectionString
    });
})
.WithName("EnvironmentTest");

app.Run();
