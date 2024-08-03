using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Tests;

internal static class DependencyInjection
{
    private static IServiceProvider? _serviceProvider;

    internal static IServiceProvider ServiceProvider
    {
        get => _serviceProvider ??= new ServiceCollection().RegisterTestServices().BuildServiceProvider();
    }

    private static IServiceCollection RegisterTestServices(this IServiceCollection services)
    {
        services.AddTrailblazorRouting();

        return services;
    }
}
