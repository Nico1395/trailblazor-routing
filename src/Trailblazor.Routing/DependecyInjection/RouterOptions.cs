using Microsoft.AspNetCore.Components;
using System.Reflection;
using Trailblazor.Routing.Constants;
using Trailblazor.Routing.Profiles;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.DependecyInjection;

public sealed class RouterOptions
{
    private RouterOptions() { }

    private readonly List<Type> _routingProfileTypes = [];
    private readonly InternalRoutingProfile _internalRoutingProfile = new();

    /// <summary>
    /// Method registers all components with an '@page' directive or <see cref="RouteAttribute"/> to the <see cref="RouterOptions"/>.
    /// </summary>
    /// <param name="assemblies">Assemblies to scan in.</param>
    /// <returns><see cref="RouterOptions"/> for further configuration.</returns>
    public RouterOptions ScanForComponentsInAssemblies(params Assembly[] assemblies)
    {
        var componentBaseType = typeof(ComponentBase);
        var routes = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => !t.IsAbstract && t.IsAssignableTo(componentBaseType) && t.GetCustomAttributes<RouteAttribute>().Any())
            .Select(type =>
            {
                var route = new Route()
                {
                    Uri = type.GetCustomAttribute<RouteAttribute>()!.Template,
                    Component = type,
                };

                route.SetMetadataValue(MetadataConstants.FromPageDirective, true);
                return route;
            })
            .ToList();

        foreach (var route in routes)
            _internalRoutingProfile.AddRoute(route);

        return this;
    }

    public RouterOptions AddProfilesFromAssemblies(params Assembly[] assemblies)
    {
        var profileBaseType = typeof(RoutingProfileBase);
        var internalProfileType = typeof(InternalRoutingProfile);

        _routingProfileTypes.AddRange(assemblies.SelectMany(a => a.GetTypes()).Where(t =>
            !t.IsAbstract &&
            t.IsAssignableTo(profileBaseType) &&
            t != internalProfileType));

        return this;
    }

    public RouterOptions AddProfile(Type profileType)
    {
        _routingProfileTypes.Add(profileType);
        return this;
    }

    public RouterOptions AddProfile<TProfile>()
        where TProfile : RoutingProfileBase
    {
        return AddProfile(typeof(TProfile));
    }

    public RouterOptions AddRoute(Route route)
    {
        _internalRoutingProfile.AddRoute(route);
        return this;
    }

    internal static RouterOptions Create()
    {
        return new RouterOptions();
    }

    /// <summary>
    /// Method returns all configured routing profile types internally.
    /// </summary>
    /// <returns>Configured routing profile types.</returns>
    internal IReadOnlyList<Type> GetProfileTypesInternal()
    {
        return _routingProfileTypes;
    }

    /// <summary>
    /// Method returns the internal navigation profile.
    /// </summary>
    /// <returns>Internal navigation profile.</returns>
    internal InternalRoutingProfile GetInternalRoutingProfile()
    {
        return _internalRoutingProfile;
    }
}
