using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Routes;

public sealed record RouterContext
{
    private RouterContext() { }

    public required string? RelativeUri { get; init; }
    public required Dictionary<string, object?> QueryParameters { get; init; }
    public required Route? Route { get; init; }
    public required RouteData RouteData { get; init; }

    internal static RouterContext Create(string relativeUri, Dictionary<string, object?> queryParameters, Route? route)
    {
        return new RouterContext()
        {
            RelativeUri = relativeUri,
            QueryParameters = queryParameters,
            Route = route,
            RouteData = CreateRouteData(route!, queryParameters),
        };
    }

    private static RouteData CreateRouteData(Route route, Dictionary<string, object?> queryParameters)
    {
        return new RouteData(route.Component, queryParameters);
    }
}
