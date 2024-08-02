using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.Profiles;

public record RoutingProfileConfiguration
{
    private readonly List<Route> _routes = [];

    private RoutingProfileConfiguration() { }

    internal static RoutingProfileConfiguration Create()
    {
        return new RoutingProfileConfiguration();
    }

    public RoutingProfileConfiguration AddRoute<TComponent>(Action<RouteBuilder<TComponent>> builderAction)
        where TComponent : IComponent
    {
        var builder = new RouteBuilder<TComponent>();
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
