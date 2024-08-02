using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

/// <summary>
/// Service manages resolving navigation profiles and their configured routes internally.
/// </summary>
internal interface IInternalRouteResolver
{
    /// <summary>
    /// Resolves configured routes internally and returns them.
    /// </summary>
    /// <returns>Resolved configured routes.</returns>
    internal List<Route> ResolveRoutes();
}
