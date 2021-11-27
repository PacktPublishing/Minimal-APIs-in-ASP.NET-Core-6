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

app.MapGet("/env-test", () =>
{
    var rootProperty = app.Configuration.GetValue<string>("RootProperty");
    var sampleVariable = app.Configuration.GetValue<string>("RootSettings:SampleVariable");
    var connectionString = app.Configuration.GetConnectionString("SqlConnection");
    return new
    {
        RootProperty = rootProperty,
        SampleVariable = sampleVariable,
        ConnetionString = connectionString
    };
})
.WithName("EnvironmentTest");

app.Run();
