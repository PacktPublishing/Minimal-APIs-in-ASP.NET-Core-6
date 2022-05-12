using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProblemDetailsSample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.TryAddSingleton<IActionResultExecutor<ObjectResult>, ProblemDetailsResultExecutor>();
builder.Services.AddProblemDetails(options =>
{
    options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
});
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

app.MapGet("/not-implemented-exception", () =>
{
    throw new NotImplementedException("This is an exception thrown from a Minimal API.");
})
    .Produces<ProblemDetails>(StatusCodes.Status501NotImplemented)
    .WithName("NotImplementedExceptions");

app.MapGet("/problems", () =>
{
    return Results.Problem(detail: "This will end up in the 'detail' field.");
})
    .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
    .WithName("Problems");

app.MapGet("/custom-error", () =>
{
    var problem = new OutOfCreditProblemDetails
    {
        Type = "https://example.com/probs/out-of-credit",
        Title = "You do not have enough credit.",
        Detail = "Your current balance is 30, but that costs 50.",
        Instance = "/account/12345/msgs/abc",
        Balance = 30.0m,
        Accounts = { "/account/12345", "/account/67890" }
    };

    return Results.Problem(problem);
})
    .Produces<OutOfCreditProblemDetails>(StatusCodes.Status400BadRequest)
    .WithName("CreditProblems");

app.Run();

public class OutOfCreditProblemDetails : ProblemDetails
{
    public OutOfCreditProblemDetails()
    {
        Accounts = new List<string>();
    }

    public decimal Balance { get; set; }

    public ICollection<string> Accounts { get; }
}
