using Trailblazor.Routing.DependencyInjection;
using Trailblazor.Routing.Profiles;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

/// <summary>
/// Service manages resolving navigation profiles and their configured routes internally.
/// </summary>
internal sealed class InternalRouteResolver(IEnumerable<IRoutingProfile> _routingProfiles) : IInternalRouteResolver
{
    /// <summary>
    /// Resolves configured routes internally and returns them.
    /// </summary>
    /// <returns>Resolved configured routes.</returns>
    public List<Route> ResolveRoutes()
    {
        var routes = _routingProfiles.SelectMany(p => p.ComposeConfigurationInternal().GetConfiguredRoutes()).ToList();
        RouteRegistrationSecurityManager.New().SecurityCheckRoutes(routes);

        return routes;
    }
}
