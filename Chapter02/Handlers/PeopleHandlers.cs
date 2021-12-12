using Chapter02.Registration;

namespace Chapter02.Handlers;

public class PeopleHandler : IEndpointRouteHandler
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/people", GetList);
        app.MapGet("/api/people/{id:guid}", Get);
        app.MapPost("/api/people", Insert);
        app.MapPut("/api/people/{id:guid}", Update);
        app.MapDelete("/api/people/{id:guid}", Delete);
    }

    private static IResult GetList(PeopleService peopleService) { return Results.NoContent(); }

    private static IResult Get(Guid id, PeopleService peopleService) { return Results.NoContent(); }

    private static IResult Insert(Person person, PeopleService people) { return Results.NoContent(); }

    private static IResult Update(Person person, PeopleService people) { return Results.NoContent(); }

    private static IResult Delete(Guid id) { return Results.NoContent(); }
}
