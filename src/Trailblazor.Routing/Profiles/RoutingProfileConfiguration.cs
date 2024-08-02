using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.Profiles;

public record RoutingProfileConfiguration
{
    private readonly IRouteParser _routeParser;
    private readonly List<Route> _routes = [];

    private RoutingProfileConfiguration(IRouteParser routeParser)
    {
        _routeParser = routeParser;
    }

    internal static RoutingProfileConfiguration Create(IRouteParser routeParser)
    {
        return new RoutingProfileConfiguration(routeParser);
    }

    public RoutingProfileConfiguration AddRoute<TComponent>(Action<RouteBuilder<TComponent>> builderAction)
        where TComponent : IComponent
    {
        var builder = new RouteBuilder<TComponent>(_routeParser);
        builderAction.Invoke(builder);

        _routes.Add(builder.Build());
        return this;
    }

    public RoutingProfileConfiguration AddRoute(Route route)
    {
        _routes.Add(route);
        return this;
    }

    internal List<Route> GetConfiguredRoutes()
    {
        return _routes;
    }
}
