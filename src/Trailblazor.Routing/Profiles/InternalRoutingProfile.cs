using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.Profiles;

/// <summary>
/// Framework internal navigation profile.
/// </summary>
/// <remarks>
/// This profile is used to internally register routes during the dependency injection registration process. This profile will be used during the startup only.
/// </remarks>
internal sealed class InternalRoutingProfile : RoutingProfileBase
{
    private readonly List<Route> _routes = [];

    internal readonly List<Type> ComponentTypes = [];

    protected sealed override void Configure(RoutingProfileConfiguration configuration)
    {
        foreach (var route in _routes)
            configuration.AddRoute(route);
    }

    /// <summary>
    /// Internally adds the specified <paramref name="route"/> to the profile.
    /// </summary>
    /// <param name="route">Route to be added.</param>
    internal void AddRoute(Route route)
    {
        route.Uri = route.Uri.TrimStart('/');
        _routes.Add(route);
    }
}
