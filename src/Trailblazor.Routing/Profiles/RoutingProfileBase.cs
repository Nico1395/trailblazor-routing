namespace Trailblazor.Routing.Profiles;

public abstract class RoutingProfileBase : IRoutingProfile
{
    public RoutingProfileConfiguration ConfigureInternal(IRouteParser routeParser)
    {
        var configuration = RoutingProfileConfiguration.Create(routeParser);
        Configure(configuration);

        return configuration;
    }

    protected abstract void Configure(RoutingProfileConfiguration configuration);
}
