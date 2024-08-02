using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.DependecyInjection;

internal sealed class RouteRegistrationSecurityManager
{
    private RouteRegistrationSecurityManager() { }

    internal static RouteRegistrationSecurityManager New()
    {
        return new RouteRegistrationSecurityManager();
    }

    public void SecurityCheckRoutes(List<Route> routes)
    {
        routes.ForEach(route =>
        {
            CheckForUrisRegisteredToMultipleComponents(route, routes);
        });
    }

    private void CheckForUrisRegisteredToMultipleComponents(Route route, List<Route> routes)
    {
        var duplicateRoutes = routes.Where(r => r != route && r.Uri == route.Uri).ToArray();
        if (duplicateRoutes.Length > 0)
            throw new UriRegisteredToMultipleRoutesException(route.Uri, duplicateRoutes.Select(r => r.Component).Concat([route.Component]).ToList());
    }
}
