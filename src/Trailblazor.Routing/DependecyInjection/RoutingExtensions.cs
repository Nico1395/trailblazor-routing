using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Profiles;

namespace Trailblazor.Routing.DependecyInjection;

public static class RoutingExtensions
{
    private static readonly Type _routingProfileInterfaceType = typeof(IRoutingProfile);

    public static IServiceCollection AddRouting(this IServiceCollection services, Action<RouterOptions> optionsAction)
    {
        services.AddScoped<IRouteProvider, RouteProvider>();
        services.AddScoped<IRouteParser, RouteParser>();
        services.AddScoped<IRouteAuthorizer, RouteAuthorizer>();
        services.AddScoped<IInternalRouteResolver, InternalRouteResolver>();

        var options = RouterOptions.Create();
        optionsAction.Invoke(options);
        services.AddSingleton(options);

        var routingProfiles = options.GetProfileTypesInternal();

        services.AddSingleton(_routingProfileInterfaceType, options.GetInternalRoutingProfile());
        foreach (var profileType in routingProfiles)
            services.AddSingleton(_routingProfileInterfaceType, profileType);

        return services;
    }

    public static IServiceCollection AddRoutingProfile<TProfile>(this IServiceCollection services)
    {
        return services.AddSingleton(_routingProfileInterfaceType, typeof(TProfile));
    }

    public static IServiceCollection AddRoutingProfile(this IServiceCollection services, Type profileType)
    {
        return services.AddSingleton(_routingProfileInterfaceType, profileType);
    }
}
