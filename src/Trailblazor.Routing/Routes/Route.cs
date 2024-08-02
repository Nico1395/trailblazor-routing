using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing.Routes;

public record Route
{
    private Dictionary<string, object?> _metadata = [];

    public string Uri { get; internal set; } = string.Empty;
    public Route? Parent { get; internal set; }
    public List<Route> Children { get; internal set; } = [];
    public required Type Component { get; init; }

    internal static Route Empty<TComponent>()
        where TComponent : IComponent
    {
        return new Route()
        {
            Component = typeof(TComponent)
        };
    }

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

    public IReadOnlyDictionary<string, object?> GetMetadata()
    {
        return _metadata.ToDictionary();
    }

    public TValue? GetMetadataValue<TValue>(string key, TValue? defaultValue = default)
    {
        if (_metadata.TryGetValue(key, out var value) && value is TValue metadataValue)
            return metadataValue;

        return defaultValue ?? default;
    }

    public void SetMetadataValue(string key, object? value)
    {
        _metadata[key] = value;
    }

    internal void MergeMetadata(Dictionary<string, object?> other)
    {
        _metadata = _metadata.Merge(other);
    }

    private bool RouteSegmentsMatch(string[] ownedUriSegments, string[] otherUriSegments)
    {
        if (ownedUriSegments.Length != otherUriSegments.Length)
            return false;

        for (var i = 0; i < otherUriSegments.Length; i++)
        {
            if (string.Compare(ownedUriSegments[i], otherUriSegments[i], StringComparison.OrdinalIgnoreCase) != 0)
                return false;
        }

        return true;
    }
}
