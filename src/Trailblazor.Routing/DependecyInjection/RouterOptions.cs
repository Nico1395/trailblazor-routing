using System.Reflection;
using Trailblazor.Routing.Profiles;
using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.DependecyInjection;

public sealed class RouterOptions
{
    private RouterOptions() { }

    private readonly List<Type> _routingProfileTypes = [];
    private readonly InternalRoutingProfile _internalRoutingProfile = new();

    public RouterOptions AddProfilesFromAssemblies(Assembly[] assemblies)
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
