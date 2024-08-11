using Microsoft.AspNetCore.Components;
using System.Reflection;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Profiles;
using Trailblazor.Routing.Routes;
using Trailblazor.Routing.Validation;

namespace Trailblazor.Routing;

internal sealed class InternalRouteCache(
    IEnumerable<IRoutingProfile> _routingProfiles,
    IInternalRouteValidator _internalRouteValidator) : IInternalRouteCache
{
    private List<Route>? _cachedRoutes;

    public List<Route> GetCachedRoutes()
    {
        return _cachedRoutes ??= ResolveRoutes();
    }

    private List<Route> ResolveRoutes()
    {
        var routes = _routingProfiles.SelectMany(p => p.ComposeConfigurationInternal().GetConfiguredRoutes()).ToList();
        routes.AddRange(ResolveComponentRoutes(routes));

        _internalRouteValidator.ValidateRoutes(routes);
        return routes;
    }

    private List<Route> ResolveComponentRoutes(List<Route> registeredRoutes)
    {
        var internalRoutingProfile = _routingProfiles.OfType<InternalRoutingProfile>().Single();
        var componentRoutes = new List<Route>();

        foreach (var component in internalRoutingProfile.ComponentTypes)
        {
            var routeAttributes = component.GetCustomAttributes<RouteAttribute>();

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

        MatchmakeRoutes(componentRoutes, registeredRoutes);

        // Only return all routes that dont have a parent. The ones that do, have to be included somewhere in the hierarchy
        return componentRoutes.Where(r => r.Parent == null).ToList();
    }

    private void MatchmakeRoutes(List<Route> componentRoutes, List<Route> registeredRoutes)
    {
        var routes = componentRoutes.Concat(registeredRoutes).ToList();

        foreach (var componentRoute in componentRoutes)
        {
            MatchmakeChildren(componentRoute, routes);
            MatchmakeParent(componentRoute, routes);
        }
    }

    private void MatchmakeChildren(Route componentRoute, List<Route> routes)
    {
        var childComponents = componentRoute.Component.GetCustomAttribute<RouteChildrenAttribute>()?.ChildrenComponents ?? [];
        var children = routes.Where(r => childComponents.Any(c => c == r.Component)).ToList();

        if (childComponents.Length != children.Count)
            throw new RouteNotFoundException($"Some child routes of route for component '{componentRoute.Component.FullName}' have not been found: Child count should be {childComponents.Length}, but is {children.Count}.");

        foreach (var child in children)
        {
            var alreadyMatchmaked = componentRoute.Children.Contains(child);
            if (alreadyMatchmaked)
                continue;

            var childAlreadyHasADifferentParent = child.Parent != null && child.Parent.Component != componentRoute.Component;
            if (childAlreadyHasADifferentParent)
                throw new RouteRelationshipException(componentRoute.Component, child.Component, child.Parent!.Component);

            componentRoute.Children.Add(child);
            child.Parent = componentRoute;
        }
    }

    private void MatchmakeParent(Route componentRoute, List<Route> routes)
    {
        var parentComponent = componentRoute.Component.GetCustomAttribute<RouteParentAttribute>()?.Parent;
        var parent = routes.SingleOrDefault(r => r.Component == parentComponent);

        var alreadyMatchmaked = parentComponent != null && parent != null && parent.Children.Contains(componentRoute);
        if (alreadyMatchmaked)
            return;

        if (parentComponent != null && parent == null)
            throw new RouteNotFoundException($"Route '{componentRoute.Uri}' has been configured with component '{parentComponent.FullName}' as its parent, however no route has been associated with the parent component.");

        componentRoute.Parent = parent;
        parent?.Children.Add(componentRoute);
    }
}
