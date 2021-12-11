namespace Chapter02.Handlers;

public static class PeopleHandler
{
    public static void MapPeopleEndpoints(this IEndpointRouteBuilder app)
    {
        //  Uncomment the following lines and comment the corresponding endpoints in Program.cs to use these handlers
        //  instead of the one defined in Program.cs.

        //app.MapGet("/people", GetList);
        //app.MapGet("/people/{id:guid}", Get);
        //app.MapPost("/people", Insert);
        //app.MapPut("/people/{id:guid}", Update);
        //app.MapDelete("/people/{id:guid}", Delete);
    }

    private static IResult GetList(PeopleService peopleService) { return Results.NoContent(); }

    private static IResult Get(Guid id, PeopleService peopleService) { return Results.NoContent(); }

    private static IResult Insert(Person Person, PeopleService people) { return Results.NoContent(); }

    private static IResult Update(Person Person, PeopleService people) { return Results.NoContent(); }

    private static IResult Delete(Guid id) { return Results.NoContent(); }
}
