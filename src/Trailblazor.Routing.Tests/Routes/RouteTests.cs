using Microsoft.Extensions.DependencyInjection;

namespace Trailblazor.Routing.Tests.Routes;

public class RouteTests
{
    [Fact]
    public void Route_FindRoute()
    {
        var routeProvider = RouteDependencyInjection.ServiceProvider.GetRequiredService<IRouteProvider>();
        var rootRoute = routeProvider.GetRoutesInHierarchy().Single();

        var firstChildRoute = rootRoute.FindRoute("child-route/first");
        Assert.NotNull(firstChildRoute);
    }
}
