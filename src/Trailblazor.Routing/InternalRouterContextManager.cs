using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Constants;
using Trailblazor.Routing.Extensions;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

/// <summary>
/// Service manages the current router context internally.
/// </summary>
internal sealed class InternalRouterContextManager(
    NavigationManager _navigationManager,
    IComponentParameterParser _componentParameterParser,
    IUriParser _uriParser,
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
        var relativeUri = _uriParser.RemoveQueryParameters(relativeUriWithParameters);
        var route = _routeProvider.FindRoute(relativeUri);
        var uriQueryParameters = _uriParser.ExtractQueryParameters(relativeUriWithParameters);
        var componentQueryParameters = GetComponentQueryParameters(route, uriQueryParameters, relativeUriWithParameters);

        return _internalRouterContext = RouterContext.New(
            relativeUriWithParameters,
            relativeUri,
            uriQueryParameters,
            componentQueryParameters,
            route);
    }

    private Dictionary<string, object?> GetComponentQueryParameters(Route? route, Dictionary<string, string> uriQueryParameters, string relativeUriWithParameters)
    {
        if (route == null)
            return [];

        if (route.GetMetadataValue(MetadataConstants.FromPageDirective, false))
        {
            return _componentParameterParser.ParseFromDirectiveQueryParameters(relativeUriWithParameters, route.Component, route.Uri);
        }
        else
        {
            return _componentParameterParser.ParseFromQueryParameters(uriQueryParameters, route.Component);
        }
    }
}
