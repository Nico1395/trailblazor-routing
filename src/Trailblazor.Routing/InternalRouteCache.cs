using Microsoft.AspNetCore.Components;
using System.Reflection;
using Trailblazor.Routing.Exceptions;
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
    private List<Route>? _cachedRoutes;

    /// <summary>
    /// Method returns cached routes.
    /// </summary>
    /// <returns>Cached routes.</returns>
    public List<Route> GetCachedRoutes()
    {
        return _cachedRoutes ??= ResolveRoutes();
    }

    /// <summary>
    /// Method resolves configured routes.
    /// </summary>
    /// <returns>Resolved routes.</returns>
    private List<Route> ResolveRoutes()
    {
        var routes = ResolveProfileRoutes().Concat(ResolveComponentRoutes()).ToList();
        var matchmakedRoutes = GetMatchmakedRoutes(routes);

        _internalRouteValidator.ValidateRoutes(matchmakedRoutes);
        return matchmakedRoutes;
    }

    private List<Route> ResolveProfileRoutes()
    {
        return _routingProfiles.SelectMany(p => p.ComposeConfigurationInternal().GetConfiguredRoutes()).ToList();
    }

    /// <summary>
    /// Method resolves routes from components with page directives that are registered with the <see cref="InternalRoutingProfile"/>.
    /// </summary>
    /// <returns>Routes resolved from components with page directives.</returns>
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

    private List<Route> GetMatchmakedRoutes(List<Route> routes)
    {
        foreach (var route in routes)
        {
            // TODO -> Evaluate relationship descriptors
            if (route.ParentDescriptor != null)
            {
                var parentRoute = routes.SingleOrDefault(r =>
                    r.Component == route.ParentDescriptor.ParentComponent &&
                    route.ParentDescriptor.ParentUri != null ? r.Uri == route.ParentDescriptor.ParentUri : true);

                if (parentRoute == null)
                    throw new RouteNotFoundException($"Route for component '{route.ParentDescriptor.ParentComponent}' and URI '{route.ParentDescriptor.ParentUri}' not found when attempting to assign it as a parent to route for component '{route.Component}' and URI '{route.Uri}'.");

                // TODO -> Is this causing double assigments?
                // TODO -> Should this be moved to matchmaking?
                route.Parent = parentRoute;
                parentRoute.Children.Add(route);
            }

            MatchmakeChildren(route, routes);
            MatchmakeParent(route, routes);
        }

        // Only return all routes that dont have a parent. The ones that do, have to be included somewhere in the hierarchy
        return routes.Where(r => r.Parent == null).ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="componentRoute"></param>
    /// <param name="routes"></param>
    /// <exception cref="RouteNotFoundException"></exception>
    /// <exception cref="RouteRelationshipException"></exception>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="componentRoute"></param>
    /// <param name="routes"></param>
    /// <exception cref="RouteNotFoundException"></exception>
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
