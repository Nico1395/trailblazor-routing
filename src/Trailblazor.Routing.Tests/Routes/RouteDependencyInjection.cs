using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.DependencyInjection;
using Trailblazor.Routing.Tests.Registration.Components;

namespace Trailblazor.Routing.Tests.Routes;

internal static class RouteDependencyInjection
{
    private static IServiceProvider? _serviceProvider;

    internal static IServiceProvider ServiceProvider
    {
        get
        {
            return _serviceProvider ??= new ServiceCollection()
                .AddCommonTestServices()
                .AddTrailblazorRouting(options =>
                {
                    options.AddRoute<DummyComponent>(r => r
                        .WithUri("root-route")
                        .WithChild<DummyComponent>(s => s
                            .WithUri("child-route/first")
                            .WithChild<DummyComponent>(k => k
                                .WithUri("child-route/second"))));
                })
                .BuildServiceProvider();
        }
    }
}
