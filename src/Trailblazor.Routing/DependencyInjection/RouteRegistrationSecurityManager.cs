using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Internal security manager security checking routes and profiles.
/// </summary>
internal sealed class RouteRegistrationSecurityManager
{
    private RouteRegistrationSecurityManager() { }

    internal static RouteRegistrationSecurityManager New()
    {
        return new RouteRegistrationSecurityManager();
    }

    /// <summary>
    /// Method security checks the specified <paramref name="routes"/>.
    /// </summary>
    /// <param name="routes">Routes to be security checked.</param>
    internal void SecurityCheckRoutes(List<Route> routes)
    {
        routes.ForEach(route =>
        {
            CheckForUrisRegisteredToMultipleComponents(route, routes);
        });
    }

    /// <summary>
    /// Method checks the specified <paramref name="route"/> for a duplicate URI.
    /// </summary>
    /// <param name="route">Route whose URI is to be checked for duplicates.</param>
    /// <param name="routes">Routes the URI of the specified <paramref name="route"/> is being checked against.</param>
    /// <exception cref="UriRegisteredToMultipleRoutesException">Thrown if duplicates have been configured.</exception>
    private void CheckForUrisRegisteredToMultipleComponents(Route route, List<Route> routes)
    {
        var duplicateRoutes = routes.Where(r => r != route && r.Uri == route.Uri).ToArray();
        if (duplicateRoutes.Length > 0)
            throw new UriRegisteredToMultipleRoutesException(route.Uri, duplicateRoutes.Select(r => r.Component).Concat([route.Component]).ToList());
    }
}
