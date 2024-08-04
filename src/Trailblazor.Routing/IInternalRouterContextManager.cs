namespace Trailblazor.Routing;

/// <summary>
/// Service manages the current router context internally.
/// </summary>
internal interface IInternalRouterContextManager
{
    /// <summary>
    /// Method returns the current router context.
    /// </summary>
    /// <returns>Current <see cref="RouterContext"/>.</returns>
    internal RouterContext GetRouterContext();

    /// <summary>
    /// Method updates the current router context.
    /// </summary>
    /// <returns>Updated <see cref="RouterContext"/>.</returns>
    internal RouterContext UpdateAndGetRouterContext();
}
