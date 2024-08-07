namespace Trailblazor.Routing.Exceptions;

/// <summary>
/// Exception expresses that no route has been associated with a given component type and/or URI.
/// </summary>
public sealed class RouteNotFoundException : Exception
{
    internal RouteNotFoundException(Type routeComponentType)
        : base($"No route that is associated with a component of type '{routeComponentType.FullName}' has been found.")
    {
    }

    internal RouteNotFoundException(string uri)
        : base($"No route with the URI '{uri}' has been found.")
    {
    }

    internal RouteNotFoundException(string uri, Type routeComponentType)
        : base($"No route that is associated with a component of type '{routeComponentType.FullName}' and has the URI '{uri}' has been found.")
    {
    }
}
