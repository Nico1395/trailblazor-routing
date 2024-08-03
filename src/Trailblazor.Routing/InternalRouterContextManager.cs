using Microsoft.AspNetCore.Components;
using Trailblazor.Routing.Extensions;

namespace Trailblazor.Routing;

internal sealed class InternalRouterContextManager(
    NavigationManager _navigationManager,
    IRouteParser _routeParser,
    IRouteProvider _routeProvider) : IInternalRouterContextManager
{
    private RouterContext _internalRouterContext = RouterContext.Empty();

    public RouterContext GetRouterContext()
    {
        return _internalRouterContext;
    }

    public RouterContext UpdateAndGetRouterContext()
    {
        var relativeUriWithParameters = _navigationManager.GetRelativeUri();
        var relativeUri = _routeParser.RemoveQueryParameters(relativeUriWithParameters);
        var queryParameters = _routeParser.ParseQueryParameters(relativeUriWithParameters);
        var route = _routeProvider.FindRoute(relativeUri);

        return _internalRouterContext = RouterContext.New(
            relativeUriWithParameters,
            relativeUri,
            queryParameters,
            route);
    }
}
