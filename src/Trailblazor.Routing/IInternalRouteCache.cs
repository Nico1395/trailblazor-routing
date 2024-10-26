using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

/// <summary>
/// Framework internal route cache.
/// </summary>
internal interface IInternalRouteCache
{
    /// <summary>
    /// Method returns cached routes in hierarchy.
    /// </summary>
    /// <returns>Cached routes.</returns>
    internal List<Route> GetCachedRoutesInHierarchy();

    /// <summary>
    /// Method returns cached routes flattened.
    /// </summary>
    /// <returns>Cached routes.</returns>
    internal List<Route> GetCachedRoutes();
}
