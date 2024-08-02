using Trailblazor.Routing.Routes;

namespace Trailblazor.Routing.Profiles;

/// <summary>
/// Framework internal navigation profile.
/// </summary>
/// <remarks>
/// This profile is used to internally register single routes during the dependency injection registrations. This profile will be used during startup only.
/// </remarks>
internal sealed class InternalRoutingProfile : RoutingProfileBase
{
    private readonly List<Route> _routes = [];

    protected sealed override void Configure(RoutingProfileConfiguration configuration)
    {
        foreach (var route in _routes)
            configuration.AddRoute(route);
    }

    internal void AddRoute(Route route)
    {
        route.Uri = route.Uri.TrimStart('/');
        _routes.Add(route);
    }
}
