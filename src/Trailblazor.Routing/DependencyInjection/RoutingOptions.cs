using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Trailblazor.Routing.Constants;
using Trailblazor.Routing.Exceptions;
using Trailblazor.Routing.Profiles;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.DependencyInjection;

/// <summary>
/// Options configuring the routing functionalities and routes.
/// </summary>
public sealed class RoutingOptions
{
    private RoutingOptions() { }

    /// <summary>
    /// Routing profile interface type.
    /// </summary>
    private readonly Type _routingProfileInterfaceType = typeof(IRoutingProfile);

    /// <summary>
    /// Registered types of routing profiles. When registering these to the <see cref="IServiceCollection"/> they will be filtered for inheritance.
    /// </summary>
    private readonly List<Type> _routingProfileTypes = [];

    /// <summary>
    /// Internal routing profile. This is being used for all manually added component types or for component types bearing the
    /// '@page' directive or <see cref="RouteAttribute"/>.
    /// </summary>
    private readonly InternalRoutingProfile _internalRoutingProfile = new();

    /// <summary>
    /// Parse options for parsing query parameters.
    /// </summary>
    public QueryParameterParseOptions QueryParameterParseOptions { get; set; } = QueryParameterParseOptions.Default();

    /// <summary>
    /// Method registers all components with an '<c>@page</c>' directive or <see cref="RouteAttribute"/> to the <see cref="RoutingOptions"/>.
    /// </summary>
    /// <remarks>
    /// Note:<br/>
    /// Components with an '<c>@page</c>' should only receive query parameters using the <see cref="ParameterAttribute"/> and either the
    /// <see cref="SupplyParameterFromQueryAttribute"/> or the <see cref="QueryParameterAttribute"/>. Routes with query parameters of following
    /// format are not supported and will malfunction:<br/>
    /// <code>
    /// @page "some-uri/{QueryParameterProperty}"
    /// </code>
    /// </remarks>
    /// <param name="assemblies">Assemblies to scan in.</param>
    /// <returns><see cref="RoutingOptions"/> for further configurations.</returns>
    public RoutingOptions ScanForComponentsInAssemblies(params Assembly[] assemblies)
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

    /// <summary>
    /// Method scans for routing profiles in the specified <paramref name="assemblies"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Only the most derived types will be registered to the <see cref="IServiceCollection"/>. If a type registers inherits from another profile, only the inheriting profile is being registered.
    /// This is done to avoid duplicate route registrations, since that would cause conflicts and thus throws exceptions intentionally.
    /// </para>
    /// <para>Abstract types will be ignored.</para>
    /// </remarks>
    /// <param name="assemblies">Assemblies to scan in for routing profiles.</param>
    /// <returns><see cref="RoutingOptions"/> for further configurations.</returns>
    public RoutingOptions AddProfilesFromAssemblies(params Assembly[] assemblies)
    {
        var profileBaseType = typeof(RoutingProfileBase);
        var internalProfileType = typeof(InternalRoutingProfile);

        _routingProfileTypes.AddRange(assemblies.SelectMany(a => a.GetTypes()).Where(t =>
            !t.IsAbstract &&
            t.IsAssignableTo(profileBaseType) &&
            t != internalProfileType));

        return this;
    }

    /// <summary>
    /// Method registers the specified <paramref name="profileType"/> to the router.
    /// </summary>
    /// <param name="profileType">Type of profile to be registered.</param>
    /// <returns><see cref="RoutingOptions"/> for further configurations.</returns>
    public RoutingOptions AddProfile(Type profileType)
    {
        if (!profileType.IsAssignableTo(_routingProfileInterfaceType))
            throw new TypeIsNotARoutingProfileException(profileType);

        if (profileType.IsAbstract || profileType.IsInterface)
            throw new AbstractRoutingProfileException(profileType);

        _routingProfileTypes.Add(profileType);
        return this;
    }

    /// <summary>
    /// Method registers the specified <typeparamref name="TProfile"/> to the router.
    /// </summary>
    /// <typeparam name="TProfile">Type of profile to be registered.</typeparam>
    /// <returns><see cref="RoutingOptions"/> for further configurations.</returns>
    public RoutingOptions AddProfile<TProfile>()
        where TProfile : RoutingProfileBase
    {
        return AddProfile(typeof(TProfile));
    }

    /// <summary>
    /// Method allows manually registering a instantiated route to the router.
    /// </summary>
    /// <param name="route"><see cref="Route"/> to be registered.</param>
    /// <returns><see cref="RoutingOptions"/> for further configurations.</returns>
    public RoutingOptions AddRoute(Route route)
    {
        _internalRoutingProfile.AddRoute(route);
        return this;
    }

    /// <summary>
    /// Method adds a route that is configured by a <paramref name="builderAction"/> to the router.
    /// </summary>
    /// <typeparam name="TComponent">Type of component representing the route.</typeparam>
    /// <param name="builderAction">Builder action for configuring the route that is to be added.</param>
    /// <returns><see cref="RoutingOptions"/> for further configurations.</returns>
    public RoutingOptions AddRoute<TComponent>(Action<RouteBuilder<TComponent>> builderAction)
        where TComponent : IComponent
    {
        var builder = new RouteBuilder<TComponent>();
        builderAction.Invoke(builder);

        _internalRoutingProfile.AddRoute(builder.Build());
        return this;
    }

    /// <summary>
    /// Method creates a new router options.
    /// </summary>
    /// <returns>New instance of router options.</returns>
    internal static RoutingOptions New()
    {
        return new RoutingOptions();
    }

    /// <summary>
    /// Method returns all configured routing profile types internally.
    /// </summary>
    /// <remarks>
    /// This method filters out profile types that other configured or scanned profile types are deriving from. This way
    /// only the most derived profiles are being used and accidentally duplicate registrations of routes are avoided.
    /// </remarks>
    /// <returns>Configured routing profile types.</returns>
    internal IReadOnlyList<Type> GetProfileTypesInternal()
    {
        return _routingProfileTypes
            .Distinct()
            .Where(t => !_routingProfileTypes.Any(other => other != t && other.IsSubclassOf(t)))
            .ToList();
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
