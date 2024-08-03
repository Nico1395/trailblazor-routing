using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

/// <summary>
/// Object represents the current context of the router.
/// </summary>
public sealed record RouterContext
{
    private RouterContext() { }

    /// <summary>
    /// Current relative URI with query parameters.
    /// </summary>
    public required string? RelativeUri { get; init; }

    /// <summary>
    /// Query parameters of the current relative URI.
    /// </summary>
    public required Dictionary<string, object?> QueryParameters { get; init; }

    /// <summary>
    /// Route associated with the current relative URI.
    /// </summary>
    public required Route? Route { get; init; }

    /// <summary>
    /// Route data for the URI.
    /// </summary>
    public required RouteData? RouteData { get; init; }

    internal static RouterContext New(string relativeUri, Dictionary<string, object?> queryParameters, Route? route)
    {
        return new RouterContext()
        {
            RelativeUri = relativeUri,
            QueryParameters = queryParameters,
            Route = route,
            RouteData = CreateRouteData(route!, queryParameters),
        };
    }

    internal static RouterContext Empty()
    {
        var route = Route.Empty<IComponent>();
        return new RouterContext()
        {
            RelativeUri = string.Empty,
            QueryParameters = [],
            Route = route,
            RouteData = CreateRouteData(route, []),
        };
    }

    private static RouteData? CreateRouteData(Route? route, Dictionary<string, object?> queryParameters)
    {
        if (route == null)
            return null;

        return new RouteData(route.Component, queryParameters);
    }
}
