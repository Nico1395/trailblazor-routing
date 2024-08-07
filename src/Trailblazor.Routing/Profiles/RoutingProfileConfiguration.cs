using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Exceptions;
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

    /// <summary>
    /// Method allows overriding component and details of a route associated with a component <typeparamref name="TComponent"/>
    /// and a specified <paramref name="uri"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of component associated with the route.</typeparam>
    /// <typeparam name="TOverrideComponent">
    /// Overriding type of component, replacing <typeparamref name="TComponent"/> as the routes associated component.
    /// </typeparam>
    /// <param name="uri">URI of the route.</param>
    /// <param name="overrideBuilderAction">Builder action for overriding the route.</param>
    /// <returns><see cref="RoutingProfileConfiguration"/> for further configurations.</returns>
    public RoutingProfileConfiguration OverrideRoute<TComponent, TOverrideComponent>(string uri, Action<RouteBuilder<TOverrideComponent>>? overrideBuilderAction = null)
        where TComponent : IComponent
        where TOverrideComponent : IComponent
    {
        var route = GetRouteForComponent<TComponent>(uri);
        var builder = new RouteBuilder<TOverrideComponent>(route);

        overrideBuilderAction?.Invoke(builder);
        var overriddenRoute = builder.Build();

        // If the route doesnt have a parent its not a cascaded route
        if (route.Parent == null)
        {
            _routes.Remove(route);
            _routes.Add(overriddenRoute);
        }

        return this;
    }

    /// <summary>
    /// Method allows editing a route associated with a component <typeparamref name="TComponent"/> and a specified <paramref name="uri"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of component associated with the route.</typeparam>
    /// <param name="uri">URI of the route.</param>
    /// <param name="editBuilderAction">Builder action for editing the route.</param>
    /// <returns><see cref="RoutingProfileConfiguration"/> for further configurations.</returns>
    public RoutingProfileConfiguration EditRoute<TComponent>(string uri, Action<RouteBuilder<TComponent>> editBuilderAction)
        where TComponent : IComponent
    {
        var route = GetRouteForComponent<TComponent>(uri);
        var builder = new RouteBuilder<TComponent>(route);

        editBuilderAction.Invoke(builder);
        var editedRoute = builder.Build();

        _routes.Remove(route);
        _routes.Add(editedRoute);

        return this;
    }

    /// <summary>
    /// Method removes a the route associated with a component <typeparamref name="TComponent"/> and a specified <paramref name="uri"/>.
    /// </summary>
    /// <typeparam name="TComponent">Type of component associated with the route.</typeparam>
    /// <param name="uri">URI of the route.</param>
    /// <returns><see cref="RoutingProfileConfiguration"/> for further configurations.</returns>
    public RoutingProfileConfiguration RemoveRoute<TComponent>(string uri)
        where TComponent : IComponent
    {
        var route = GetRouteForComponent<TComponent>(uri);
        _routes.Remove(route);

        return this;
    }

    internal List<Route> GetConfiguredRoutes()
    {
        return _routes;
    }

    private Route GetRouteForComponent<TComponent>(string uri)
        where TComponent : IComponent
    {
        uri = uri.TrimStart('/');

        var componentType = typeof(TComponent);
        var route = _routes
            .Select(r =>
            {
                var foundRoute = r.FindRoute(uri);
                if (foundRoute?.Component != componentType)
                    return null;

                return foundRoute;
            })
            .Where(r => r != null)
            .SingleOrDefault()
                ?? throw new RouteNotFoundException(uri, componentType);

        return route;
    }
}
