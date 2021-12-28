using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProblemDetailsSample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.TryAddSingleton<IActionResultExecutor<ObjectResult>, ProblemDetailsResultExecutor>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseProblemDetails();
app.UseHttpsRedirection();

app.MapGet("/internal-server-error", () =>
{
    throw new ArgumentNullException("taggia-parameter", "Taggia has an error");
})
    .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError)
    .WithName("internal-server-error");

app.Run();