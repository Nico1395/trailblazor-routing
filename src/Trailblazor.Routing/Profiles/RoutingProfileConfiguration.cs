using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.Profiles;

/// <summary>
/// Configuration of an <see cref="IRoutingProfile"/>.
/// </summary>
public record RoutingProfileConfiguration
{
    private readonly List<Route> _routes = [];

    private RoutingProfileConfiguration() { }

    internal static RoutingProfileConfiguration Create()
    {
        return new RoutingProfileConfiguration();
    }

    /// <summary>
    /// Method adds a configurable route to the <see cref="RoutingProfileConfiguration"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="builderAction">Builder action for configuring the route.</param>
    /// <returns><see cref="RoutingProfileConfiguration"/> for further configurations.</returns>
    public RoutingProfileConfiguration AddRoute<TComponent>(Action<RouteBuilder<TComponent>> builderAction)
        where TComponent : IComponent
    {
        var builder = new RouteBuilder<TComponent>();
        builderAction.Invoke(builder);

        _routes.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Method adds the specified <paramref name="route"/> to the <see cref="RoutingProfileConfiguration"/>.
    /// </summary>
    /// <param name="route">Route to be added.</param>
    /// <returns><see cref="RoutingProfileConfiguration"/> for further configurations.</returns>
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
