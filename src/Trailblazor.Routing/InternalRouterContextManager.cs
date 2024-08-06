using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing;

/// <summary>
/// Service manages the current router context internally.
/// </summary>
internal sealed class InternalRouterContextManager(
    NavigationManager _navigationManager,
    IQueryParameterParser _queryParameterParser,
    IRouteParser _routeParser,
    IRouteProvider _routeProvider) : IInternalRouterContextManager
{
    private RouterContext _internalRouterContext = RouterContext.Empty();

    /// <summary>
    /// Method returns the current router context.
    /// </summary>
    /// <returns>Current <see cref="RouterContext"/>.</returns>
    public RouterContext GetRouterContext()
    {
        return _internalRouterContext;
    }

    /// <summary>
    /// Method updates the current router context.
    /// </summary>
    /// <returns>Updated <see cref="RouterContext"/>.</returns>
    public RouterContext UpdateAndGetRouterContext()
    {
        var relativeUriWithParameters = _navigationManager.GetRelativeUri();
        var relativeUri = _routeParser.RemoveQueryParameters(relativeUriWithParameters);
        var route = _routeProvider.FindRoute(relativeUri);
        var uriQueryParameters = _routeParser.ExtractQueryParameters(relativeUriWithParameters);
        var componentQueryParameters = route != null ? _queryParameterParser.ParseToComponentParameters(uriQueryParameters, route.Component) : [];

        return _internalRouterContext = RouterContext.New(
            relativeUriWithParameters,
            relativeUri,
            uriQueryParameters,
            componentQueryParameters,
            route);
    }
}
