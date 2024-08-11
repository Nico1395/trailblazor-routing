using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing.Routes;

/// <summary>
/// Object represents a route.
/// </summary>
public record Route
{
    private Dictionary<string, object?> _metadata = [];

    internal RouteParentDescriptor? ParentDescriptor { get; set; }
    internal List<RouteChildDescriptor> ChildDescriptors { get; set; } = [];

    /// <summary>
    /// URI of the route. This is unique in the system.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Uri)]
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
    public Type Component { get; internal set; } = null!;

    /// <summary>
    /// Internally creates an empty <see cref="Route"/> from the given <typeparamref name="TComponent"/> type-parameter.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <returns>Empty route leading to the <typeparamref name="TComponent"/>.</returns>
    public static Route Empty<TComponent>()
        where TComponent : IComponent
    {
        var componentType = typeof(TComponent);
        if (componentType.IsAbstract || componentType.IsInterface)
            throw new AbstractComponentException(componentType);

        return new Route() { Component = componentType, };
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
    /// Method finds all routes that are associated with the specified <paramref name="componentType"/>.
    /// </summary>
    /// <param name="componentType">Type of component whose associated routes are to be returned.</param>
    /// <returns>Routes associated with the specified <paramref name="componentType"/>.</returns>
    public List<Route> FindRoutes(Type componentType)
    {
        var foundRoutes = new List<Route>();
        AccumulateRoutesForType(componentType, foundRoutes);

        return foundRoutes;
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
    /// Method fetches the routes metadata value for the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the desired value.</param>
    /// <param name="defaultValue">Optional default value in case the desired value has not been found.</param>
    /// <returns>Found metadata value for the specified <paramref name="key"/>.</returns>
    public object? GetMetadataValue(string key, object? defaultValue = default)
    {
        return _metadata.TryGetValue(key, out var value) ? value : (defaultValue ?? default);
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
    /// Method removes the routes metadata value with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key of the metadata value that is to be removed.</param>
    public void RemoveMetadataValue(string key)
    {
        _metadata.Remove(key);
    }

    /// <summary>
    /// Method determines whether the route has a metadata value with the specified <paramref name="key"/>.
    /// </summary>
    /// <param name="key">Key that is to be checked for.</param>
    /// <returns><see langword="true"/> if the routes metadata contains a value with the specified <paramref name="key"/>.</returns>
    public bool HasMetadataValue(string key)
    {
        return _metadata.ContainsKey(key);
    }

    /// <summary>
    /// Method internally merges the routes metadata with <paramref name="other"/> metadata.
    /// </summary>
    /// <param name="other">Other metadata to be merged into the routes metadata.</param>
    internal void MergeMetadata(Dictionary<string, object?> other)
    {
        _metadata = _metadata.Merge(other);
    }

    /// <summary>
    /// Method accumulates routes associated with the specified <paramref name="componentType"/> in the
    /// specified <paramref name="foundRoutes"/> list.
    /// </summary>
    /// <param name="componentType">Type of component whose associated routes are to be returned.</param>
    /// <param name="foundRoutes">List of routes associated with the <paramref name="componentType"/>.</param>
    private void AccumulateRoutesForType(Type componentType, List<Route> foundRoutes)
    {
        if (Component == componentType)
            foundRoutes.Add(this);

        foreach (var subPage in Children)
            subPage.AccumulateRoutesForType(componentType, foundRoutes);
    }
}
