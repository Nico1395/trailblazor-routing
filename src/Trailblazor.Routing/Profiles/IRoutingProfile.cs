namespace Trailblazor.Routing.Profiles;

internal interface IRoutingProfile
{
    internal RoutingProfileConfiguration ConfigureInternal(IRouteParser routeParser);
}
