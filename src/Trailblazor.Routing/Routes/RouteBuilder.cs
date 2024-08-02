using Microsoft.AspNetCore.Components;

namespace Trailblazor.Routing.Routes;

/// <summary>
/// Standard route builder for building <see cref="Route"/>s.
/// </summary>
/// <typeparam name="TComponent">Type of component representing the route.</typeparam>
public sealed class RouteBuilder<TComponent>
    where TComponent : IComponent
{
    private readonly Route _route = Route.Empty<TComponent>();

    internal RouteBuilder() { }

    internal RouteBuilder(Route parentRoute)
    {
        _route.Parent = parentRoute;
    }

    /// <summary>
    /// Method adds the specified <paramref name="value"/> for the <paramref name="key"/> to the routes metadata.
    /// </summary>
    /// <param name="key">Key of the metadata value.</param>
    /// <param name="value">Value for the <paramref name="key"/>.</param>
    /// <returns>Route builder for further configurations.</returns>
    public RouteBuilder<TComponent> WithMetadataValue(string key, object? value)
    {
        _route.SetMetadataValue(key, value);
        return this;
    }

    /// <summary>
    /// Method adds the specified <paramref name="metadata"/> to the route.
    /// </summary>
    /// <param name="metadata">Metadata to be added.</param>
    /// <returns>Route builder for further configurations.</returns>
    public RouteBuilder<TComponent> WithMetadata(Dictionary<string, object?> metadata)
    {
        _route.MergeMetadata(metadata);
        return this;
    }

    /// <summary>
    /// Method sets the specified <paramref name="uri"/> to be the routes URI.
    /// </summary>
    /// <param name="uri">The routes URI.</param>
    /// <returns>Route builder for further configurations.</returns>
    public RouteBuilder<TComponent> WithUri(string uri)
    {
        _route.Uri = uri.TrimStart('/');
        return this;
    }

    /// <summary>
    /// Method adds a child route to the route.
    /// </summary>
    /// <typeparam name="TChildComponent">Type of child route.</typeparam>
    /// <param name="builderAction">Builder action for the configuration of the child route.</param>
    /// <returns>Route builder for further configurations.</returns>
    public RouteBuilder<TComponent> WithChild<TChildComponent>(Action<RouteBuilder<TChildComponent>> builderAction)
        where TChildComponent : IComponent
    {
        var builder = new RouteBuilder<TChildComponent>(_route);

        builderAction.Invoke(builder);
        _route.Children.Add(builder.Build());

        return this;
    }

    /// <summary>
    /// Method builds and returns the configured routes.
    /// </summary>
    /// <returns>Configured route.</returns>
    internal Route Build()
    {
        return _route;
    }
}
