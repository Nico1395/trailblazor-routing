using Microsoft.Extensions.DependencyInjection;
using Trailblazor.Routing.Profiles;

namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Static class contains dependency injection extension methods.
/// </summary>
public static class RoutingDependencyInjection
{
    private static readonly Type _routingProfileInterfaceType = typeof(IRoutingProfile);

    /// <summary>
    /// Extension method registers the Trailblazor routing framework to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/> the routing services and profiles are being registered to.</param>
    /// <param name="optionsAction">Optional action allowing the configuration of the <see cref="RoutingOptions"/> used to configure the router.</param>
    /// <returns><see cref="IServiceCollection"/> for further configurations.</returns>
    public static IServiceCollection AddTrailblazorRouting(this IServiceCollection services, Action<RoutingOptions>? optionsAction = null)
    {
        services.AddScoped<IInternalRouteResolver, InternalRouteResolver>();
        services.AddScoped<IInternalRouterContextManager, InternalRouterContextManager>();
        services.AddScoped<IQueryParameterParser, QueryParameterParser>();

        services.AddScoped<IRouteProvider, RouteProvider>();
        services.AddScoped<IUriParser, UriParser>();
        services.AddScoped<IRouterContextAccessor, RouterContextAccessor>();
        services.AddScoped<INavigator, Navigator>();

        var options = RoutingOptions.New();
        optionsAction?.Invoke(options);
        services.AddScoped<IRoutingOptionsAccessor>(serviceProvider => new RoutingOptionsAccessor(options));

        var routingProfiles = options.GetProfileTypesInternal();
        services.AddSingleton(_routingProfileInterfaceType, options.GetInternalRoutingProfile());
        foreach (var profileType in routingProfiles)
            services.AddSingleton(_routingProfileInterfaceType, profileType);

        return services;
    }
}
