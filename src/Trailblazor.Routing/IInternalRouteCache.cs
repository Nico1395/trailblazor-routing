using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

internal interface IInternalRouteCache
{
    internal List<Route> GetCachedRoutes();
}
