using Microsoft.Extensions.DependencyInjection;

namespace Trailblazor.Routing.Tests.Routes;

public class RouteProviderTests
{
    [Fact]
    public void RouteProvider_FindRoute()
    {
        var routeProvider = RouteDependencyInjection.ServiceProvider.GetRequiredService<IRouteProvider>();

        var route = routeProvider.FindRoute("child-route/second");
        Assert.NotNull(route);
    }
}
