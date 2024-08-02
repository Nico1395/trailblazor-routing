using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing;

public interface IRouteAuthorizer
{
    public bool Authorize(Route route);
}
