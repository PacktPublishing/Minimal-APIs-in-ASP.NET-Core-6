using Chapter02.Registration;

namespace Chapter02.Handlers;

public class PeopleHandler : IRouteEndpointHandler
{
    public void Map(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/people", GetList);
        app.MapGet("/api/people/{id:guid}", Get);
        app.MapPost("/api/people", Insert);
        app.MapPut("/api/people/{id:guid}", Update);
        app.MapDelete("/api/people/{id:guid}", Delete);
    }

    // Comment the Map method above, uncomment the following one (and the corresponding invocation in Program.cs),
    // then make the class static and delete the IRouteEndpointHandler inheritance if you want to explicitly register the endpoints
    // instead of using the automatic registration.
    //public static void MapPeopleEndpoints(this IEndpointRouteBuilder app)
    //{
    //    app.MapGet("/api/people", GetList);
    //    app.MapGet("/api/people/{id:guid}", Get);
    //    app.MapPost("/api/people", Insert);
    //    app.MapPut("/api/people/{id:guid}", Update);
    //    app.MapDelete("/api/people/{id:guid}", Delete);
    //}

    private static IResult GetList(PeopleService peopleService) { return Results.NoContent(); }

    private static IResult Get(Guid id, PeopleService peopleService) { return Results.NoContent(); }

    private static IResult Insert(Person Person, PeopleService people) { return Results.NoContent(); }

    private static IResult Update(Person Person, PeopleService people) { return Results.NoContent(); }

    private static IResult Delete(Guid id) { return Results.NoContent(); }
}
