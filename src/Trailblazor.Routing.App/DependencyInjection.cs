using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Trailblazor.Routing.DependencyInjection;

namespace Trailblazor.Routing.App;

public static class DependencyInjection
{
    public static IServiceCollection AddApp(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddTrailblazorRouting(options =>
        {
            assemblies = assemblies.Concat([typeof(DependencyInjection).Assembly]).ToArray();
            options.AddProfilesFromAssemblies(assemblies);
        });

        return services;
    }
}
