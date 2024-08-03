namespace Trailblazor.Routing;

/// <summary>
/// Service provides access to the current <see cref="RouterContext"/>.
/// </summary>
public interface IRouterContextAccessor
{
    /// <summary>
    /// Method gets the current router context.
    /// </summary>
    /// <returns>Current <see cref="RouterContext"/>.</returns>
    public RouterContext GetRouterContext();
}
