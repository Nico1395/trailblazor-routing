using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.Validation;

/// <summary>
/// Service validates routes.
/// </summary>
internal sealed class InternalRouteValidator : IInternalRouteValidator
{
    /// <summary>
    /// Method validates specified <paramref name="routes"/> for integrity and functionality.
    /// </summary>
    /// <param name="routes">Routes to be validated.</param>
    public void ValidateRoutes(List<Route> routes)
    {
        routes.ForEach(route =>
        {
            ValidateUriAssignment(route, routes);
            ValidateRelationships(route);
        });
    }

    /// <summary>
    /// Method checks the specified <paramref name="route"/> for a duplicate URI.
    /// </summary>
    /// <param name="route">Route whose URI is to be checked for duplicates.</param>
    /// <param name="routes">Routes the URI of the specified <paramref name="route"/> is being checked against.</param>
    /// <exception cref="UriRegisteredToMultipleRoutesException">Thrown if duplicates have been configured.</exception>
    private void ValidateUriAssignment(Route route, List<Route> routes)
    {
        var duplicateRoutes = routes.Where(r => r != route && r.Uri == route.Uri).ToArray();
        if (duplicateRoutes.Length > 0)
            throw new UriRegisteredToMultipleRoutesException(route.Uri, duplicateRoutes.Select(r => r.Component).Concat([route.Component]).ToList());
    }

    /// <summary>
    /// Method validates the configured <paramref name="route"/>s relationships.
    /// </summary>
    /// <param name="route">Route whose relationships are to be validated.</param>
    private void ValidateRelationships(Route route)
    {
        var circularDependentChild = GetCircularDependentRoute(route);
        if (circularDependentChild != null)
            throw new RouteRelationshipException($"Routes for components '{route.Component.FullName}' (parent) and '{circularDependentChild.Component.FullName}' (child) have a circular relationship.");
    }

    /// <summary>
    /// Method gets the route the <paramref name="parentRoute"/> has a circular relationship with.
    /// </summary>
    /// <param name="parentRoute">Parent route whose children are to be checked for circular dependencies.</param>
    /// <returns>Route the <paramref name="parentRoute"/> has a circular relationship with.</returns>
    private Route? GetCircularDependentRoute(Route parentRoute)
    {
        foreach (var child in parentRoute.Children)
        {
            if (child == parentRoute)
                return child;

            var circularDependentChild = GetCircularDependentRoute(child);
            if (circularDependentChild != null)
                return circularDependentChild;
        }

        return null;
    }
}
