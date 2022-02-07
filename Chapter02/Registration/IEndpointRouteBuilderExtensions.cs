using System.Reflection;

namespace Chapter02.Registration;

public static class IEndpointRouteBuilderExtensions
{
    public static void MapEndpoints(this IEndpointRouteBuilder app, Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(app);
        ArgumentNullException.ThrowIfNull(assembly);

        var endpointRouteHandlerInterfaceType = typeof(IEndpointRouteHandler);

        var endpointRouteHandlerTypes = assembly.GetTypes().Where(t =>
            t.IsClass && !t.IsAbstract && !t.IsGenericType
            && t.GetConstructor(Type.EmptyTypes) != null
            && endpointRouteHandlerInterfaceType.IsAssignableFrom(t));

        foreach (var endpointRouteHandlerType in endpointRouteHandlerTypes)
        {
            var instantiatedType = (IEndpointRouteHandler)Activator.CreateInstance(endpointRouteHandlerType)!;
            instantiatedType.MapEndpoints(app);
        }
    }
}