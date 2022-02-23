using System.Globalization;
using Chapter09.Swagger;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<CultureAwareOperationFilter>();
});

var supportedCultures = new CultureInfo[] { new("en"), new("it") };

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SupportedCultures = supportedCultures;
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

app.Run();