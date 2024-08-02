using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

internal sealed class RouteAuthorizer : IRouteAuthorizer
{
    public bool Authorize(Route route)
    {
        var routePermissions = route.GetMetadataValue<string[]>("permissions", []);
        if (routePermissions?.Length == 0)
            return true;

        // TODO -> AuthenticationSessionProvider and then use that to check on permissions. However the async nature of JSInterop in the provider could be tricky.
        return true;
    }
}
