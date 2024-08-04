using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Trailblazor.Routing.Tests;

internal static class CommonTestDependecyInjection
{
    internal static IServiceCollection AddCommonTestServices(this IServiceCollection services)
    {
        services.AddScoped<NavigationManager, TestNavigationManager>();

        return services;
    }
}
