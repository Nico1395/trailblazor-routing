using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing.Tests.Parsing;

internal static class ParsingDependencyInjection
{
    private static IServiceProvider? _serviceProvider;

    internal static IServiceProvider ServiceProvider
    {
        get => _serviceProvider ??= new ServiceCollection().AddCommonTestServices().RegisterTestServices().BuildServiceProvider();
    }

    private static IServiceCollection RegisterTestServices(this IServiceCollection services)
    {
        services.AddTrailblazorRouting();

        return services;
    }
}
