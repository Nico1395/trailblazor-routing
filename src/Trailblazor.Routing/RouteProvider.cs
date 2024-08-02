using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Extensions;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

internal sealed class RouteProvider(
    IRouteParser _routeParser,
    IServiceProvider _serviceProvider,
    NavigationManager _navigationManager,
    IInternalRouteResolver _internalRouteResolver) : IRouteProvider
{
    private List<Route>? _routes;
    private List<Route>? _moduleRoutes;
    private IRouteAuthorizer? _routeAuthorizer;
    private bool _routeAuthorizerResolved;

    public IReadOnlyList<Route> GetRoutes()
    {
        return _routes ??= _internalRouteResolver.ResolveRoutes();
    }

    public IReadOnlyList<Route> GetAuthorizedRoutes()
    {
        return FilterAuthorizedInternal(GetRoutes());
    }

    public IReadOnlyList<Route> GetModules()
    {
        return _moduleRoutes ??= GetRoutes().Where(p => p.GetMetadataValue<bool>("is-module")).ToList();
    }

    public IReadOnlyList<Route> GetAuthorizedModules()
    {
        return FilterAuthorizedInternal(GetModules());
    }

    public Route? GetCurrentModule()
    {
        var currentRelativeUri = _navigationManager.GetRelativeUri();
        return FindModuleInternal(currentRelativeUri);
    }

    public Route? GetCurrentRoute()
    {
        var currentRelativeUri = _navigationManager.GetRelativeUri();
        return FindRoute(currentRelativeUri);
    }

    public Route? FindRoute(string relativeUri)
    {
        relativeUri = _routeParser.RemoveQueryParameters(relativeUri);
        return GetRoutes().Select(p => p.FindRoute(relativeUri)).Where(p => p != null).SingleOrDefault();
    }

    public bool IsCurrentRoute(Route page)
    {
        return GetCurrentRoute() == page;
    }

    private Route? FindModuleInternal(string relativeUri)
    {
        relativeUri = _routeParser.RemoveQueryParameters(relativeUri);
        return GetModules().SingleOrDefault(m => m.FindRoute(relativeUri) != null);
    }

    private IReadOnlyList<Route> FilterAuthorizedInternal(IReadOnlyList<Route> routes)
    {
        _routeAuthorizer ??= !_routeAuthorizerResolved ? _serviceProvider.GetService<IRouteAuthorizer>() : null;
        _routeAuthorizerResolved = true;

        if (_routeAuthorizer == null)
            return routes;

        return routes.Where(_routeAuthorizer.Authorize).ToList();
    }
}
