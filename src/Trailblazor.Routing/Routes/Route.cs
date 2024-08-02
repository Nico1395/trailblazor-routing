using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing.Routes;

/// <summary>
/// Object represents a route.
/// </summary>
public record Route
{
    /// <summary>
    /// Dictionary of metadata values.
    /// </summary>
    private Dictionary<string, object?> _metadata = [];

    /// <summary>
    /// URI of the route. This is unique in the system.
    /// </summary>
    public string Uri { get; internal set; } = string.Empty;

    /// <summary>
    /// Optional parent route of the route.
    /// </summary>
    public Route? Parent { get; internal set; }

    /// <summary>
    /// Child routes of the route.
    /// </summary>
    public List<Route> Children { get; internal set; } = [];

    /// <summary>
    /// Type of component representing the route.
    /// </summary>
    public required Type Component { get; init; }

    /// <summary>
    /// Internally creates an empty <see cref="Route"/> from the given <typeparamref name="TComponent"/> type-parameter.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <returns>Empty route leading to the <typeparamref name="TComponent"/>.</returns>
    internal static Route Empty<TComponent>()
        where TComponent : IComponent
    {
        return new Route()
        {
            Component = typeof(TComponent)
        };
    }

    /// <summary>
    /// Method finds a route with the given <paramref name="uri"/> in the routes children. Returns itself if the <paramref name="uri"/>
    /// matches the routes <see cref="Uri"/>.
    /// </summary>
    /// <param name="uri">URI to be searched for.</param>
    /// <returns>Route with the desired <paramref name="uri"/> if found.</returns>
    public Route? FindRoute(string uri)
    {
        if (Uri == uri)
            return this;

        foreach (var subPage in Children)
        {
            var activePage = subPage.FindRoute(uri);
            if (activePage != null)
                return activePage;
        }

        return null;
    }

    /// <summary>
    /// Method gets all of the routes metadata.
    /// </summary>
    /// <returns>Metadata of the route.</returns>
    public IReadOnlyDictionary<string, object?> GetMetadata()
    {
        return _metadata.ToDictionary();
    }

    /// <summary>
    /// Method fetches the routes metadata value for the specified <paramref name="key"/> and casts it into the <typeparamref name="TValue"/>.
    /// </summary>
    /// <typeparam name="TValue">Type of metadata value.</typeparam>
    /// <param name="key">Key of the desired value.</param>
    /// <param name="defaultValue">Optional default value in case the desired value has not been found.</param>
    /// <returns>Found metadata value for the specified <paramref name="key"/>.</returns>
    public TValue? GetMetadataValue<TValue>(string key, TValue? defaultValue = default)
    {
        if (_metadata.TryGetValue(key, out var value) && value is TValue metadataValue)
            return metadataValue;

        return defaultValue ?? default;
    }

    /// <summary>
    /// Method sets the metadata <paramref name="value"/> for the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the <paramref name="value"/> to be set.</param>
    /// <param name="value">Value to be set.</param>
    public void SetMetadataValue(string key, object? value)
    {
        _metadata[key] = value;
    }

    /// <summary>
    /// Method internally merges the routes metadata with <paramref name="other"/> metadata.
    /// </summary>
    /// <param name="other">Other metadata to be merged into the routes metadata.</param>
    internal void MergeMetadata(Dictionary<string, object?> other)
    {
        _metadata = _metadata.Merge(other);
    }
}
