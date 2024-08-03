namespace Trailblazor.Routing;

/// <summary>
/// Internal service manages the current router context.
/// </summary>
internal interface IInternalRouterContextManager
{
    internal RouterContext GetRouterContext();
    internal RouterContext UpdateAndGetRouterContext();
}
