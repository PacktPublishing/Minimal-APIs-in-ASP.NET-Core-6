using LoggingSourceGenerator;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<LogGenerator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/start-log", (PostData data, LogGenerator logGenerator) =>
{
    logGenerator.StartEndpointSignal("start-log", data);
})
.WithName("StartLog");
app.Run();

internal record PostData(DateTime Date, string Name);