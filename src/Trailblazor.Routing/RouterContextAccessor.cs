namespace Trailblazor.Routing;

/// <summary>
/// Service provides access to the current <see cref="RouterContext"/>.
/// </summary>
internal sealed class RouterContextAccessor(IInternalRouterContextManager _routerContextManager) : IRouterContextAccessor
{
    /// <summary>
    /// Method gets the current router context.
    /// </summary>
    /// <returns>Current <see cref="RouterContext"/>.</returns>
    public RouterContext GetRouterContext()
    {
        return _routerContextManager.GetRouterContext();
    }
}
