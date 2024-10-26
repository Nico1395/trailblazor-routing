using Microsoft.AspNetCore.Components;
using System.Reflection;
using Trailblazor.Routing.Profiles;
using Trailblazor.Routing.Routes;
using Trailblazor.Routing.Validation;

namespace Trailblazor.Routing;

/// <summary>
/// Framework internal route cache.
/// </summary>
internal sealed class InternalRouteCache(
    IEnumerable<IRoutingProfile> _routingProfiles,
    IInternalRouteValidator _internalRouteValidator) : IInternalRouteCache
{
    private List<Route>? _cachedRoutesInHieararchy;
    private List<Route>? _cachedRoutes;

    /// <summary>
    /// Method returns cached routes.
    /// </summary>
    /// <returns>Cached routes.</returns>
    public List<Route> GetCachedRoutesInHierarchy()
    {
        return _cachedRoutesInHieararchy ??= ResolveRoutesInHierarchy();
    }

    /// <summary>
    /// Method returns cached routes flattened.
    /// </summary>
    /// <returns>Cached routes.</returns>
    public List<Route> GetCachedRoutes()
    {
        return _cachedRoutes ??= ResolveProfileRoutes().Concat(ResolveComponentRoutes()).ToList();
    }

    private List<Route> ResolveRoutesInHierarchy()
    {
        var routesInHierarchy = BuildHierarchyRoutes();

        _internalRouteValidator.ValidateRoutes(routesInHierarchy);
        return routesInHierarchy;
    }

    private IEnumerable<Route> ResolveProfileRoutes()
    {
        return _routingProfiles.SelectMany(p => p.ComposeConfigurationInternal().GetConfiguredRoutes());
    }

    private List<Route> ResolveComponentRoutes()
    {
        var internalRoutingProfile = _routingProfiles.OfType<InternalRoutingProfile>().Single();
        var componentRoutes = new List<Route>();

        foreach (var component in internalRoutingProfile.ComponentTypes)
        {
            var routeUris = component.GetCustomAttributes<RouteAttribute>().Select(r => r.Template.TrimStart('/')).Distinct();
            var routeMetadata = component.GetCustomAttributes<RouteMetadataAttribute>().ToDictionary(k => k.MetadataKey, v => v.MetadataValue);

            foreach (var routeUri in routeUris)
            {
                var route = new Route()
                {
                    Uri = routeUri,
                    Component = component,
                };

                route.MergeMetadata(routeMetadata);
                componentRoutes.Add(route);
            }
        }

        return componentRoutes;
    }

    public List<Route> BuildHierarchyRoutes()
    {
        var allResolvedFlattenedCachedRoutes = GetCachedRoutes();

        var uriLookup = allResolvedFlattenedCachedRoutes.ToDictionary(route => route.Uri, route => route);
        var typeLookup = allResolvedFlattenedCachedRoutes.GroupBy(route => route.Component).ToDictionary(g => g.Key, g => g.ToList());
        var topLevelRoutes = new List<Route>();

        foreach (var route in allResolvedFlattenedCachedRoutes)
        {
            ProcessParent(route, uriLookup, typeLookup);
            ProcessChildren(route, uriLookup, typeLookup);

            if (route.Parent == null)
                topLevelRoutes.Add(route);
        }

        return topLevelRoutes;
    }

    private void ProcessParent(Route route, Dictionary<string, Route> uriLookup, Dictionary<Type, List<Route>> typeLookup)
    {
        var parentDescriptor = route.ParentDescriptor;
        if (parentDescriptor == null)
            return;

        var parentRoute = FindRouteByDescriptor(parentDescriptor.ParentUri, parentDescriptor.ParentComponent, uriLookup, typeLookup)
            ?? throw new Exception($"Parent route not found for route '{route.Uri}'. Check URI '{parentDescriptor.ParentUri}' or component '{parentDescriptor.ParentComponent}'.");

        route.Parent = parentRoute;
        parentRoute.Children.Add(route);
    }

    private void ProcessChildren(Route route, Dictionary<string, Route> uriLookup, Dictionary<Type, List<Route>> typeLookup)
    {
        foreach (var childDescriptor in route.ChildDescriptors)
        {
            var childRoute = FindRouteByDescriptor(childDescriptor.ChildUri, childDescriptor.ChildComponent, uriLookup, typeLookup)
                ?? throw new Exception($"Child route not found for route '{route.Uri}'. Check URI '{childDescriptor.ChildUri}' or component '{childDescriptor.ChildComponent}'.");

            childRoute.Parent = route;
            route.Children.Add(childRoute);
        }
    }

    private Route? FindRouteByDescriptor(string? uri, Type component, Dictionary<string, Route> uriLookup, Dictionary<Type, List<Route>> typeLookup)
    {
        if (!string.IsNullOrEmpty(uri))
        {
            if (uriLookup.TryGetValue(uri, out var route))
                return route;

            return null;
        }

        if (typeLookup.TryGetValue(component, out var candidates) && candidates.Count == 1)
            return candidates.First();

        // Ambiguous or missing route for this component type
        return null;
    }
}
