using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

public interface IRouteProvider
{
    public IReadOnlyList<Route> GetRoutes();
    public IReadOnlyList<Route> GetAuthorizedRoutes();
    public IReadOnlyList<Route> GetModules();
    public IReadOnlyList<Route> GetAuthorizedModules();
    public Route? GetCurrentModule();
    public Route? GetCurrentRoute();
    public Route? FindRoute(string relativeUri);
    public bool IsCurrentRoute(Route page);
}
