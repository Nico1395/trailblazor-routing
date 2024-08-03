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
        var relativeUri = _navigationManager.GetRelativeUri();
        var queryParameters = _routeParser.ParseQueryParameters(relativeUri);
        var cleanRelativeUri = _routeParser.RemoveQueryParameters(relativeUri);
        var route = _routeProvider.FindRoute(cleanRelativeUri);

        return _internalRouterContext = RouterContext.New(
            relativeUri,
            queryParameters,
            route);
    }
}
