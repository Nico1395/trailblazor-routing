﻿using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

/// <summary>
/// Service provides registered routes.
/// </summary>
public interface IRouteProvider
{
    /// <summary>
    /// Method returns all registered routes in hierarchy.
    /// </summary>
    /// <returns>All registered routes.</returns>
    public IReadOnlyList<Route> GetRoutesInHierarchy();

    /// <summary>
    /// Method returns all registered routes flattened.
    /// </summary>
    /// <returns>All registered routes.</returns>
    public IReadOnlyList<Route> GetRoutes();

    /// <summary>
    /// Method returns the route for the current URI.
    /// </summary>
    /// <remarks>
    /// Standard query parameters with standard query parameter syntax will be removed and the resulting URI will be used
    /// to find the current route. Make sure to use query parameters with standard syntax ('my-route?myParameter=myValue').
    /// </remarks>
    /// <returns>Current route if any route has been registered for the current URI.</returns>
    public Route? GetCurrentRoute();

    /// <summary>
    /// Method finds the route registered to the specified <paramref name="relativeUri"/>.
    /// </summary>
    /// <param name="relativeUri">Relative URI whose associated route is to be searched for.</param>
    /// <returns>Route if found.</returns>
    public Route? FindRoute(string relativeUri);

    /// <summary>
    /// Method finds all routes that are associated with the specified <paramref name="componentType"/>.
    /// </summary>
    /// <param name="componentType">Type of component whose associated routes are to be returned.</param>
    /// <returns>Routes associated with the specified <paramref name="componentType"/>.</returns>
    public List<Route> FindRoutes(Type componentType);

    /// <summary>
    /// Method determines whether the specified <paramref name="route"/> is the current route.
    /// </summary>
    /// <param name="route">Route to be checked for.</param>
    /// <returns><see langword="true"/> if the specified <paramref name="route"/> is the current route.</returns>
    public bool IsCurrentRoute(Route route);
}
