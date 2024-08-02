using Trailblazor.Routing.Profiles;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

internal sealed class InternalRouteResolver(
    IRouteParser _routeParser,
    IEnumerable<IRoutingProfile> _routingProfiles) : IInternalRouteResolver
{
    private List<Route>? _configuredRoutes;

    public List<Route> ResolveRoutes()
    {
        return _configuredRoutes ??= ResolveInternal();
    }

    private List<Route> ResolveInternal()
    {
        return _routingProfiles.SelectMany(p => p.ConfigureInternal(_routeParser).GetConfiguredRoutes()).ToList();
    }
}
