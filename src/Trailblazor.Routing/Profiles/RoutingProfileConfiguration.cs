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

    public RoutingProfileConfiguration RemoveRouteByUri(string uri)
    {
        var route = _routes.FirstOrDefault(r => r.Uri == uri);
        if (route != null)
            _routes.Remove(route);

        return this;
    }

    public RoutingProfileConfiguration RemoveRouteByMetadataValue(string key, object? value)
    {
        var route = _routes.FirstOrDefault(r => r.GetMetadata().Any(k => k.Key == key && k.Value == value));
        if (route != null)
            _routes.Remove(route);

        return this;
    }

    // TODO -> Editing

    internal List<Route> GetConfiguredRoutes()
    {
        return _routes;
    }
}
